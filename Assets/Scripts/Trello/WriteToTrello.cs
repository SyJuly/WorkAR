﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WriteToTrello : MonoBehaviour {

    TrelloAPI trelloAPI;

    TrelloCard cardToBeInserted;

    /*------------------Singleton---------------------->>*/
    private static WriteToTrello _instance;

    public static WriteToTrello Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    /*<<------------------Singleton-----------------------*/

    void Start()
    {
        trelloAPI = TrelloAPI.Instance.gameObject.GetComponent<TrelloAPI>();
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
