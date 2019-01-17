using AsImpL;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ReadFromTrello {

    TrelloAPI trelloAPI;

    private TrelloCard[] allCards = null;
    private TrelloList[] lists = null;
    public Dictionary<string, TrelloList> cardsByList { get; private set; }
    public string boardTitle { get; private set; }

    private bool areListsReady = false;
    private bool areCardsReady = false;

    public ReadFromTrello(TrelloAPI api)
    {
        trelloAPI = api;
    }

    private void GetTrelloBoardElements()
    {
        Utility.Instance.StartCoroutine(GetBoardTitle(0));
        Utility.Instance.StartCoroutine(GetBoardLists(1));
        Utility.Instance.StartCoroutine(GetBoardCards(2));
    }

    public IEnumerator GetTrelloBoardData()
    {
        GetTrelloBoardElements();
        while (!(areListsReady & areCardsReady))
        {
            yield return new WaitForSeconds(1);
        }
        AssignCardsToList();
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
                    yield return Utility.Instance.StartCoroutine(GetCardAttachment(card));
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
            yield return Utility.Instance.StartCoroutine(GetImage(card, trelloAttachmentResponse.trelloAttachment.previews[0].url));
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
