using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReadFromTrello : MonoBehaviour {

    TrelloAPI trelloAPI;

    TrelloBoardManager manager;

    private TrelloCard[] allCards = null;
    private TrelloList[] lists = null;
    public Dictionary<string, TrelloList> cardsByList;
    public string boardTitle = null;

    //TODO:private bool[] status;

    void Start()
    {
        trelloAPI = GetComponent<TrelloAPI>();
        manager = GetComponentInChildren<TrelloBoardManager>();
        StartCoroutine(UpdateTrelloBoard());
    }

    IEnumerator UpdateTrelloBoard()
    {
        while (true)
        {
            StartCoroutine(GetBoardTitle());
            StartCoroutine(GetBoardLists());
            StartCoroutine(GetBoardCards());
            yield return new WaitForSeconds(10);
            AssignCardsToList();
            manager.UpdateTrelloBoard();
        }
    }

    IEnumerator GetBoardTitle()
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

    IEnumerator GetBoardLists()
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
        }
    }

    IEnumerator GetBoardCards()
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
            allCards = trelloCardsResponse.cards;
        }
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
        for (int i=0; i < allCards.Length; i++)
        {
            TrelloCard currentCard = allCards[i];
            TrelloList listOfCurrentCard = orderCardsByList[currentCard.idList];
            listOfCurrentCard.cards.Add(currentCard);
        }
        cardsByList = orderCardsByList;
    }
}
