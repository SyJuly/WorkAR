using HoloToolkit.Unity.InputModule.Utilities.Interactions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ReadFromTrello : Reader {

    TrelloAPI trelloAPI;

    TrelloBoardManager manager;

    //TODO remove
    [SerializeField]
    InteractionModel targetModelObject;

    private TrelloCard[] allCards = null;
    private TrelloList[] lists = null;
    public Dictionary<string, TrelloList> cardsByList;
    public string boardTitle = null;

    private bool areListsReady = false;
    private bool areCardsReady = false;

    void Start()
    {
        trelloAPI = TrelloAPI.Instance.gameObject.GetComponent<TrelloAPI>();
        manager = GetComponentInChildren<TrelloBoardManager>();
        targetModelObject = Object.FindObjectOfType<InteractionModel>();
        StartCoroutine(UpdateTrelloBoard());
    }

    IEnumerator UpdateTrelloBoard()
    {
        Get3DModell();
        while (true)
        {
            StartCoroutine(GetBoardTitle(0));
            StartCoroutine(GetBoardLists(1));
            StartCoroutine(GetBoardCards(2));
            yield return new WaitForSeconds(10);
            if (areListsReady & areCardsReady)
            {
                AssignCardsToList();
            }
            manager.UpdateTrelloBoard(cardsByList);
        }
    }

    void Get3DModell()
    {
        StartCoroutine(GetModellCard());
    }

    IEnumerator GetModellCard()
    {
        UnityWebRequest ModellCardRequest = trelloAPI.GetModellCardHTTPRequest();
        ModellCardRequest.chunkedTransfer = false;
        ModellCardRequest.timeout = 100000;

        yield return ModellCardRequest.SendWebRequest();
        if (ModellCardRequest.isNetworkError || ModellCardRequest.isHttpError)
        {
            Debug.Log("An error occured receiving modell card: " + ModellCardRequest.responseCode);
        }
        else
        {
            string responseToJSON = "{\"card\":" + ModellCardRequest.downloadHandler.text + "}";
            TrelloCard card= JsonUtility.FromJson<TrelloCard>(responseToJSON);
            yield return StartCoroutine(GetModellLinkAttachment(card));
        }
    }

    IEnumerator GetModellLinkAttachment(TrelloCard card)
    {
        UnityWebRequest ModellLinkAttachmentRequest = trelloAPI.GetLinkAttachmentHTTPRequest();
        ModellLinkAttachmentRequest.chunkedTransfer = false;
        ModellLinkAttachmentRequest.timeout = 100000;

        yield return ModellLinkAttachmentRequest.SendWebRequest();
        if (ModellLinkAttachmentRequest.isNetworkError || ModellLinkAttachmentRequest.isHttpError)
        {
            Debug.Log("An error occured receiving modell attachment link: " + ModellLinkAttachmentRequest.responseCode);
        }
        else
        {
            string responseToJSON = "{\"trelloAttachments\":" + ModellLinkAttachmentRequest.downloadHandler.text + "}";
            TrelloCardAttachmentsResponse attachments = JsonUtility.FromJson<TrelloCardAttachmentsResponse>(responseToJSON);
            Debug.Log("attachment link? " + attachments.trelloAttachments[0].url);
            yield return StartCoroutine(GetModell(attachments.trelloAttachments[0].url));
        }
    }

    IEnumerator GetModell(string url)
    {
        UnityWebRequest ModellRequest = UnityWebRequest.Get(url);
        ModellRequest.chunkedTransfer = false;
        ModellRequest.timeout = 100000;

        yield return ModellRequest.SendWebRequest();
        if (ModellRequest.isNetworkError || ModellRequest.isHttpError)
        {
            Debug.Log("An error occured receiving modell attachment link: " + ModellRequest.responseCode);
        }
        else
        {
            byte[] modell = ModellRequest.downloadHandler.data;
            ImportObj(modell);
        }
    }

    void ImportObj(byte[] modell)
    {
        string path = Application.persistentDataPath + "/modell.obj";

        //Write some text to the test.txt file

        File.WriteAllBytes(path, modell);

        byte[] b = File.ReadAllBytes(path);

        Mesh objMesh = new ObjImporter().ImportFile(Application.persistentDataPath + "/modell.obj");
        Vector3[] vertices = objMesh.vertices;

        targetModelObject.GetComponent<MeshFilter>().mesh = objMesh;
        targetModelObject.GetComponent<MeshCollider>().enabled = true;
        //targetModelObject.GetComponent<MeshCollider>().mesh = objMesh;
    }

    static Action<GameObject> objectLoadedCallback;
    static ObjectImporter objectLoadedImporter;


    public void LoadObjectFromFile(ObjectImporter importer, Transform parent, Action<GameObject> callback)
    {
        string path = LoadFile("Load 3D-Model file", "3D-Model", "obj");
        if (path == "") return;

        objectLoadedCallback = callback;
        objectLoadedImporter = importer;
        objectLoadedImporter.ImportedModel += OnImportFinished;
        objectLoadedImporter.ImportFile(path, parent, new ImportOptions());
        objectLoadedImporter.GetComponent<ObjectImporterLoadingIcon>().StartLoadingIcon();
    }



    IEnumerator GetBoardTitle(int statusIndex)
    {
        UnityWebRequest BoardTitleRequest = trelloAPI.GetBoardTitleHTTPRequest();
        BoardTitleRequest.chunkedTransfer = false;
        BoardTitleRequest.timeout = 100000;

        yield return BoardTitleRequest.SendWebRequest();
        if (BoardTitleRequest.isNetworkError || BoardTitleRequest.isHttpError)
        {
            Debug.Log("An error occured receiving events: " + BoardTitleRequest.responseCode);
        }
        else
        {
            TrelloBoard board = JsonUtility.FromJson<TrelloBoard>(BoardTitleRequest.downloadHandler.text);
            boardTitle = board.name;
        }
    }

    IEnumerator GetBoardLists(int statusIndex)
    {
        UnityWebRequest BoardListsRequest = trelloAPI.GetBoardListsHTTPRequest();
        BoardListsRequest.chunkedTransfer = false;
        BoardListsRequest.timeout = 100000;

        yield return BoardListsRequest.SendWebRequest();
        if (BoardListsRequest.isNetworkError || BoardListsRequest.isHttpError)
        {
            Debug.Log("An error occured receiving events: " + BoardListsRequest.responseCode);
        }
        else
        {
            string responseToJSON = "{\"lists\":" + BoardListsRequest.downloadHandler.text + "}";
            TrelloLists trelloListsResponse = JsonUtility.FromJson<TrelloLists>(responseToJSON);
            lists = trelloListsResponse.lists;
            areListsReady = true;
        }
    }

    IEnumerator GetBoardCards(int statusIndex)
    {
        UnityWebRequest BoardCardsRequest = trelloAPI.GetAllCardsHTTPRequest();
        BoardCardsRequest.chunkedTransfer = false;
        BoardCardsRequest.timeout = 100000;

        yield return BoardCardsRequest.SendWebRequest();
        if (BoardCardsRequest.isNetworkError || BoardCardsRequest.isHttpError)
        {
            Debug.Log("An error occured receiving events: " + BoardCardsRequest.responseCode);
        }
        else
        {
            string responseToJSON = "{\"cards\":" + BoardCardsRequest.downloadHandler.text + "}";
            TrelloCards trelloCardsResponse = JsonUtility.FromJson<TrelloCards>( responseToJSON);
            TrelloCard[] cardsWithAttachmentData = trelloCardsResponse.cards;
            foreach (TrelloCard card in cardsWithAttachmentData)
            {
                if(card.idAttachmentCover != null && card.idAttachmentCover != "")
                {
                    yield return StartCoroutine(GetCardAttachment(card));
                }
            }
            allCards = cardsWithAttachmentData;
            areCardsReady = true;
        }
    }

    IEnumerator GetCardAttachment(TrelloCard card)
    {
        UnityWebRequest CardAttachmentRequest = trelloAPI.GetCardCoverAttachmentHTTPRequest(card);
        CardAttachmentRequest.chunkedTransfer = false;
        CardAttachmentRequest.timeout = 100000;

        yield return CardAttachmentRequest.SendWebRequest();
        if (CardAttachmentRequest.isNetworkError || CardAttachmentRequest.isHttpError)
        {
            Debug.Log("An error occured receiving events: " + CardAttachmentRequest.responseCode);
        }
        else
        {
            string responseToJSON = "{\"trelloAttachment\":" + CardAttachmentRequest.downloadHandler.text + "}";
            TrelloAttachmentResponse trelloAttachmentResponse = JsonUtility.FromJson<TrelloAttachmentResponse>(responseToJSON);
            yield return StartCoroutine(GetImage(card, trelloAttachmentResponse.trelloAttachment.previews[0].url));
        }
    }

    IEnumerator GetImage(TrelloCard card, string url) {
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(url);
        yield return textureRequest.SendWebRequest();
        card.attachment = DownloadHandlerTexture.GetContent(textureRequest);
    }

    void AssignCardsToList()
    {
        Dictionary<string, TrelloList> orderCardsByList = new Dictionary<string, TrelloList>();
        for (int i = 0; i < lists.Length; i++)
        {
            TrelloList currentList = lists[i];
            currentList.cards = new List<TrelloCard>();
            orderCardsByList.Add(currentList.id, currentList);
        }
        for (int i = 0; i < allCards.Length; i++)
        {
            TrelloCard currentCard = allCards[i];
            TrelloList listOfCurrentCard = orderCardsByList[currentCard.idList];
            listOfCurrentCard.cards.Add(currentCard);
        }
        cardsByList = orderCardsByList;
        areListsReady = false;
        areCardsReady = false;
    }
}
