using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System;

public class GoogleCalendar : MonoBehaviour
{
    GoogleCrendentials credentials;

    [Serializable]
    public struct GoogleCrendentials
    {
        public string client_id;
        public string auth_uri;
        public string token_uri;
        public string client_secret;
        public string redirect_uri;
        public string calendar_events_code;
        public string calendar_endpoint;
        public string calendar_id;
    }

    [Serializable]
    public class Access_Token_Response
    {
        public string access_token;
        public string expires_in;
        public string refresh_token;
        public string scope;
        public string token_type;
    }

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
        StartCoroutine(GetCalendar());
    }

    IEnumerator GetCalendar()
    {

        List<IMultipartFormSection> parameters = new List<IMultipartFormSection>();
        parameters.Add( new MultipartFormDataSection("client_secret", credentials.client_secret));
        parameters.Add( new MultipartFormDataSection("grant_type", "authorization_code"));
        parameters.Add( new MultipartFormDataSection("code", credentials.calendar_events_code));
        parameters.Add( new MultipartFormDataSection("redirect_uri", credentials.redirect_uri));
        parameters.Add( new MultipartFormDataSection("client_id", credentials.client_id));
        Debug.Log(credentials.client_id);


        UnityWebRequest wwwPost = UnityWebRequest.Post(credentials.token_uri, parameters);
        yield return wwwPost.SendWebRequest();

        if (wwwPost.isNetworkError || wwwPost.isHttpError)
        {
            Debug.Log(wwwPost.error);
        }
        else
        {
            string response = wwwPost.downloadHandler.text;
            // Show results as text
            Debug.Log(response);

            Access_Token_Response atr = JsonUtility.FromJson<Access_Token_Response>(response);

            StartCoroutine(GetCalendarEvents(atr));

        }
    }

    IEnumerator GetCalendarEvents(Access_Token_Response atr)
    {
        UnityWebRequest AlleCalendarEventsRequest = UnityWebRequest.Get(credentials.calendar_endpoint + credentials.calendar_id + "/events?access_token=" + atr.access_token);
        yield return AlleCalendarEventsRequest.SendWebRequest();
        if (AlleCalendarEventsRequest.isNetworkError || AlleCalendarEventsRequest.isHttpError)
        {
            Debug.Log(AlleCalendarEventsRequest.error);
        }
        else
        {
            Debug.Log(AlleCalendarEventsRequest.downloadHandler.text);
        }
    }
}