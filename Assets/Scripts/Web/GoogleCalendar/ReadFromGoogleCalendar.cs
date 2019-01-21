using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net.Http;

public class ReadFromGoogleCalendar : IRefreshedTokenRequester
{
    private GoogleCalendarAPI calendarAPI;

    private System.Action<GoogleCalendarEvent[]> eventsUpdater;

    public ReadFromGoogleCalendar(GoogleCalendarAPI api)
    {
        calendarAPI = api;
    }

    public void SetEventsUpdater(System.Action<GoogleCalendarEvent[]> eventsUpdater)
    {
        this.eventsUpdater = eventsUpdater;
    }

    public IEnumerator GetCalendarEvents()
    {
        UnityWebRequest AlleCalendarEventsRequest = calendarAPI.GetCalendarEventsHTTPRequest();
        AlleCalendarEventsRequest.chunkedTransfer = false;
        AlleCalendarEventsRequest.timeout = 100000;

        yield return AlleCalendarEventsRequest.SendWebRequest();
        if (AlleCalendarEventsRequest.isNetworkError || AlleCalendarEventsRequest.isHttpError)
        {
            
            if (AlleCalendarEventsRequest.responseCode == 401)
            {
                Debug.Log("Refreshed token");
                calendarAPI.RefreshAccessToken(this);
            }
            Debug.Log("An error occured receiving events: " + AlleCalendarEventsRequest.responseCode);
        }
        else
        {
            GoogleCalendarEventsResponse eventsResponse = JsonUtility.FromJson<GoogleCalendarEventsResponse>(AlleCalendarEventsRequest.downloadHandler.text);
            eventsUpdater?.Invoke(eventsResponse.items);
        }
    }

    IEnumerator Test()
    {
        UnityWebRequest AlleCalendarEventsRequest = UnityWebRequest.Get("https://www.google.com");
        AlleCalendarEventsRequest.chunkedTransfer = false;
        AlleCalendarEventsRequest.timeout = 100000;

        yield return AlleCalendarEventsRequest.SendWebRequest();
        if (AlleCalendarEventsRequest.isNetworkError || AlleCalendarEventsRequest.isHttpError)
        {
            Debug.Log("error testing: " + AlleCalendarEventsRequest.responseCode);
        }
        else
        {
            Debug.Log("succeeded testing: " + AlleCalendarEventsRequest.responseCode);
        }
    }

    public void AfterRefreshedToken()
    {
        Utility.Instance.StartCoroutine(GetCalendarEvents());
    }
}