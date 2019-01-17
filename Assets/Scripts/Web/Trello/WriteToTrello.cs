using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WriteToTrello {

    TrelloAPI trelloAPI;

    TrelloCard cardToBeInserted;


    public WriteToTrello(TrelloAPI api)
    {
        trelloAPI = api;
    }

    public void SendCardToTrello(TrelloCard cardToInsert)
    {
        cardToBeInserted = cardToInsert;
        Utility.Instance.StartCoroutine(InsertCard());
    }

    public void SendReorderedCardToTrello(string cardId, string listId)
    {
        Utility.Instance.StartCoroutine(ReorderCard(cardId, listId));
    }

    public void SendDeleteCardToTrello(string cardId)
    {
        Utility.Instance.StartCoroutine(DeleteCard(cardId));
    }

    IEnumerator InsertCard()
    {
        UnityWebRequest InsertCardRequest;
        if (cardToBeInserted.attachment != null)
        {
            InsertCardRequest = trelloAPI.InsertCard(cardToBeInserted, cardToBeInserted.attachment.EncodeToPNG());
        } else
        {
            InsertCardRequest = trelloAPI.InsertCard(cardToBeInserted);
        }
        InsertCardRequest.timeout = 90000000;
        yield return InsertCardRequest.SendWebRequest();
        if (InsertCardRequest.isNetworkError || InsertCardRequest.isHttpError)
        {
            Debug.Log("Error occured inserting new Trello card: " + InsertCardRequest.responseCode);
        }
        else
        {
            cardToBeInserted = null;
        }
        
    }

    IEnumerator ReorderCard(string cardId, string listId)
    {
        UnityWebRequest ReorderCardRequest= trelloAPI.GetAssignCardToListHTTPRequest(cardId, listId);
        ReorderCardRequest.timeout = 90000000;
        yield return ReorderCardRequest.SendWebRequest();
        if (ReorderCardRequest.isNetworkError || ReorderCardRequest.isHttpError)
        {
            Debug.Log("Error occured reordering new Trello card: " + ReorderCardRequest.responseCode);
        }
        else
        {
            Debug.Log("Reordered card.");
        }
    }

    IEnumerator DeleteCard(string cardId)
    {
        UnityWebRequest ArchiveCardRequest = trelloAPI.GetArchiveCardtHTTPRequest(cardId);
        ArchiveCardRequest.timeout = 90000000;
        yield return ArchiveCardRequest.SendWebRequest();
        if (ArchiveCardRequest.isNetworkError || ArchiveCardRequest.isHttpError)
        {
            Debug.Log("Error occured archiving Trello card: " + ArchiveCardRequest.responseCode);
        }
        else
        {
            Debug.Log("Archived card.");
        }
    }
}
