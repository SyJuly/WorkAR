using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System;

public class GoogleCalendar : MonoBehaviour
{
    GoogleCrendentials credentials;

    void ReadGoogleCalendarCredentials()
    {
        string path = "Credentials/credentials_googlecalendar_workAR.json";

        StreamReader reader = new StreamReader(path);
        string fileText = reader.ReadToEnd();
        reader.Close();

        credentials = JsonUtility.FromJson<GoogleCrendentials>(fileText);
    }

    void Start()
    {
        ReadGoogleCalendarCredentials();
        StartCoroutine(GetCalendarEvents());
    }

    IEnumerator GetCalendarEvents()
    {
        string path = "Credentials/access_token.json";

        StreamReader reader = new StreamReader(path);
        string fileText = reader.ReadToEnd();
        reader.Close();

        GoogleAccessToken gat = JsonUtility.FromJson<GoogleAccessToken>(fileText);

        UnityWebRequest AlleCalendarEventsRequest = UnityWebRequest.Get(credentials.calendar_endpoint + credentials.calendar_id + "/events?access_token=" + gat.access_token);
        yield return AlleCalendarEventsRequest.SendWebRequest();
        if (AlleCalendarEventsRequest.isNetworkError || AlleCalendarEventsRequest.isHttpError)
        {
            Debug.Log(AlleCalendarEventsRequest.error);
            if (AlleCalendarEventsRequest.responseCode == 401)
            {
                //StartCoroutine(RefreshTokenRequest());
            }
        }
        else
        {
            Debug.Log(AlleCalendarEventsRequest.downloadHandler.text);
        }
    }
}