using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WriteToGoogleCalendar : MonoBehaviour {

    GoogleCalendarAPI calendarAPI;

    bool tryAgain = false;

    void Start()
    {
        calendarAPI = GetComponent<GoogleCalendarAPI>();
    }

    private void Update()
    {
        if (tryAgain)
        {
            StartCoroutine(InsertEvent());
            tryAgain = false;
        }
    }

    public void SendEventToCalendar(string eventName)
    {
        StartCoroutine(InsertEvent());
        
    }

    IEnumerator InsertEvent()
    {
        Debug.Log("send new event......DRAAAAAAAAAAAAAAAAGOOOOOOOOOOOOOONS");
        GoogleCalendarEvent eventToInsert = new GoogleCalendarEvent("blaevent", DateTime.Now, DateTime.Now);
        UnityWebRequest InserEventRequest = calendarAPI.InsertCalendarevent(eventToInsert);

        yield return InserEventRequest.SendWebRequest();
        if (InserEventRequest.isNetworkError || InserEventRequest.isHttpError)
        {
            if (InserEventRequest.responseCode == 401)
            {
                Debug.Log("Refreshed token");
                calendarAPI.RefreshAccessToken();
                tryAgain = true;
            }
        }
        else
        {
            Debug.Log("success sending post");
        }
    }
}
