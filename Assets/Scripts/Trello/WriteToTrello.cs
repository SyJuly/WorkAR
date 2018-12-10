using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WriteToTrello : MonoBehaviour {

    TrelloAPI trelloAPI;

    TrelloCard cardToBeInserted;

    void Start()
    {
        trelloAPI = GetComponent<TrelloAPI>();
    }

    public void SendCardToTrello(TrelloCard cardToInsert)
    {
        cardToBeInserted = cardToInsert;
        StartCoroutine(InsertCard());
    }

    IEnumerator InsertCard()
    {
        UnityWebRequest InsertCardRequest = trelloAPI.InsertCard(cardToBeInserted);
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
}
