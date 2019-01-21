using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WriteToGoogleCalendar : IRefreshedTokenRequester {

    private GoogleCalendarAPI calendarAPI;

    private GoogleCalendarEvent eventToBeInserted;

    public WriteToGoogleCalendar(GoogleCalendarAPI api)
    {
        calendarAPI = api;
    }

    public void SendEventToCalendar(GoogleCalendarEvent eventToInsert)
    {
        eventToBeInserted = eventToInsert;
        Utility.Instance.StartCoroutine(InsertEvent());
    }

    IEnumerator InsertEvent()
    {
        UnityWebRequest InserEventRequest = calendarAPI.InsertCalendarevent(eventToBeInserted);
        InserEventRequest.timeout = 90000000;
        yield return InserEventRequest.SendWebRequest();
        if (InserEventRequest.isNetworkError || InserEventRequest.isHttpError)
        {
            if (InserEventRequest.responseCode == 401)
            {
                Debug.Log("Refreshed token");
                calendarAPI.RefreshAccessToken(this);
            }
        }
        else
        {
            eventToBeInserted = null;
        }
    }

    public void AfterRefreshedToken()
    {
        if(eventToBeInserted != null)
        {
            Utility.Instance.StartCoroutine(InsertEvent());
        }
    }
}
