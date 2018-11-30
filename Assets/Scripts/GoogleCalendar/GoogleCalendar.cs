using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using TMPro;

public class GoogleCalendar : MonoBehaviour
{
    GoogleCrendentials credentials;

    GoogleAccessToken gat;

    [SerializeField]
    TextMeshPro errorField;

    public GoogleCalendarEvent[] events = null;

    void Start()
    {
        ReadGoogleCalendarCredentials();
        ReadGoogleCalendarAccessToken();
        StartCoroutine(UpdateCalendar());
        errorField.text = "bro??";
    }

    IEnumerator UpdateCalendar()
    {
        while(true)
        {
            StartCoroutine(GetCalendarEvents());
            yield return new WaitForSeconds(15);
        }
    }

    string getUTCTime()
    {
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp.ToString();
    }


    IEnumerator GetCalendarEvents()
    {
        Debug.Log("Try get new calendar events");
        errorField.text = "Get calendar events";
        UnityWebRequest AlleCalendarEventsRequest = UnityWebRequest.Get(credentials.calendar_endpoint + credentials.calendar_id + "/events?access_token=" + gat.access_token + "&t=" + getUTCTime());
        AlleCalendarEventsRequest.chunkedTransfer = false;

        yield return AlleCalendarEventsRequest.SendWebRequest();
        errorField.text = "returned web request";
        if (AlleCalendarEventsRequest.isNetworkError || AlleCalendarEventsRequest.isHttpError)
        {
            errorField.text = "Error: " + AlleCalendarEventsRequest.error;
            if (AlleCalendarEventsRequest.responseCode == 401)
            {
                Debug.Log("Refreshed token");
                errorField.text = "Refreshed token";
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
        parameters.Add(new MultipartFormDataSection("t", getUTCTime()));

        UnityWebRequest NewAccessTokenRequest = UnityWebRequest.Post(credentials.token_uri, parameters);
        NewAccessTokenRequest.chunkedTransfer = false;
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
        errorField.text = "ReadGoogleCalendarCredentials";
        String path = "Credentials/credentials_googlecalendar_workAR";
        credentials = JsonUtility.FromJson<GoogleCrendentials>(ReadFile(path));
    }

    void ReadGoogleCalendarAccessToken()
    {
        errorField.text = "ReadGoogleCalendarAccessToken";
        String path = "Credentials/access_token";
        gat = JsonUtility.FromJson<GoogleAccessToken>(ReadFile(path));
    }

    String ReadFile(String path)
    {
        TextAsset txtAsset = (TextAsset)Resources.Load(path, typeof(TextAsset));
        return txtAsset.text;
    }
}