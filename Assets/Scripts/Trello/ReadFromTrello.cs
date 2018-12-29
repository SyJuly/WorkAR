using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReadFromTrello : Reader {

    TrelloAPI trelloAPI;

    TrelloBoardManager manager;

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
        StartCoroutine(UpdateTrelloBoard());
    }

    IEnumerator UpdateTrelloBoard()
    {
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
