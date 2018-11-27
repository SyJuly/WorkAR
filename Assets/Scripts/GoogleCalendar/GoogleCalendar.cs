using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System;

public class GoogleCalendar : MonoBehaviour
{
    GoogleCrendentials credentials;

    GoogleAccessToken gat;

    public GoogleCalendarEvent[] events = null;

    void Start()
    {
        ReadGoogleCalendarCredentials();
        ReadGoogleCalendarAccessToken();
        StartCoroutine(GetCalendarEvents());
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

        UnityWebRequest AlleCalendarEventsRequest = UnityWebRequest.Get(credentials.calendar_endpoint + credentials.calendar_id + "/events?access_token=" + gat.access_token);
        yield return AlleCalendarEventsRequest.SendWebRequest();
        if (AlleCalendarEventsRequest.isNetworkError || AlleCalendarEventsRequest.isHttpError)
        {
            Debug.Log(AlleCalendarEventsRequest.error);
            if (AlleCalendarEventsRequest.responseCode == 401)
            {
                Debug.Log("Refreshed token");
                StartCoroutine(GetNewAccessToken(gat.refresh_token));
            }
        }
        else
        {
            GoogleCalendarEventsResponse eventsResponse = JsonUtility.FromJson<GoogleCalendarEventsResponse>(AlleCalendarEventsRequest.downloadHandler.text);
            events = eventsResponse.items;
        }
    }

    IEnumerator GetNewAccessToken(String refreshToken)
    {
        List<IMultipartFormSection> parameters = new List<IMultipartFormSection>();
        parameters.Add(new MultipartFormDataSection("client_secret", credentials.client_secret));
        parameters.Add(new MultipartFormDataSection("grant_type", "refresh_token"));
        parameters.Add(new MultipartFormDataSection("refresh_token", refreshToken));
        parameters.Add(new MultipartFormDataSection("client_id", credentials.client_id));

        UnityWebRequest NewAccessTokenRequest = UnityWebRequest.Post(credentials.token_uri, parameters);
        yield return NewAccessTokenRequest.SendWebRequest();
        if (NewAccessTokenRequest.isNetworkError || NewAccessTokenRequest.isHttpError)
        {
            Debug.Log(NewAccessTokenRequest.downloadHandler.text);
        }
        else
        {
            gat = JsonUtility.FromJson<GoogleAccessToken>(NewAccessTokenRequest.downloadHandler.text);
            StartCoroutine(GetCalendarEvents());
        }
    }

    void ReadGoogleCalendarCredentials()
    {
        string path = "Credentials/credentials_googlecalendar_workAR.json";
        credentials = JsonUtility.FromJson<GoogleCrendentials>(ReadFile(path));
    }

    void ReadGoogleCalendarAccessToken()
    {
        string path = "Credentials/access_token.json";
        gat = JsonUtility.FromJson<GoogleAccessToken>(ReadFile(path));
    }

    String ReadFile(String path)
    {
        StreamReader reader = new StreamReader(path);
        string fileText = reader.ReadToEnd();
        reader.Close();
        return fileText;
    }
}