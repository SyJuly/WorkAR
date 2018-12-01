﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ReadFromGoogleCalendar : MonoBehaviour, IRefreshedTokenRequester
{
    GoogleCalendarAPI calendarAPI;

    public GoogleCalendarEvent[] events = null;

    void Start()
    {
        calendarAPI = GetComponent<GoogleCalendarAPI>();
        StartCoroutine(UpdateCalendar());
    }

    IEnumerator UpdateCalendar()
    {
        while(true)
        {
            StartCoroutine(GetCalendarEvents());
            yield return new WaitForSeconds(15);
        }
    }

    IEnumerator GetCalendarEvents()
    {
        Debug.Log("Try get new calendar events");
        UnityWebRequest AlleCalendarEventsRequest = calendarAPI.GetCalendarEventsHTTPRequest();
        AlleCalendarEventsRequest.chunkedTransfer = false;

        yield return AlleCalendarEventsRequest.SendWebRequest();
        if (AlleCalendarEventsRequest.isNetworkError || AlleCalendarEventsRequest.isHttpError)
        {
            if (AlleCalendarEventsRequest.responseCode == 401)
            {
                Debug.Log("Refreshed token");
                calendarAPI.RefreshAccessToken(this);
            }
        }
        else
        {
            GoogleCalendarEventsResponse eventsResponse = JsonUtility.FromJson<GoogleCalendarEventsResponse>(AlleCalendarEventsRequest.downloadHandler.text);
            events = eventsResponse.items;
        }
    }

    public void AfterRefreshedToken()
    {
        StartCoroutine(GetCalendarEvents());
    }
}