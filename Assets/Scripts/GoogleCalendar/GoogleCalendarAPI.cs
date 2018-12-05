using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleCalendarAPI : MonoBehaviour {

    GoogleCrendentials credentials;
    
    GoogleAuthentification authentificator;

    // Use this for initialization
    void Awake () {
        authentificator = new GoogleAuthentification();
        ReadGoogleCalendarCredentials();
    }

    void Start()
    {
        authentificator = new GoogleAuthentification();
        ReadGoogleCalendarCredentials();
    }

    public void RefreshAccessToken(IRefreshedTokenRequester requester)
    {
        List<IMultipartFormSection> parameters = new List<IMultipartFormSection>();
        parameters.Add(new MultipartFormDataSection("client_secret", credentials.client_secret));
        parameters.Add(new MultipartFormDataSection("grant_type", "refresh_token"));
        parameters.Add(new MultipartFormDataSection("refresh_token", authentificator.gat.refresh_token));
        parameters.Add(new MultipartFormDataSection("client_id", credentials.client_id));
        parameters.Add(new MultipartFormDataSection("t", getUTCTime()));

        StartCoroutine(authentificator.GetNewAccessToken(UnityWebRequest.Post(credentials.token_uri, parameters), requester));
    }

    public UnityWebRequest GetCalendarEventsHTTPRequest()
    {
        return UnityWebRequest.Get(credentials.calendar_endpoint + credentials.calendar_id + "/events?access_token=" + authentificator.gat.access_token + "&t=" + getUTCTime());
    }

    public UnityWebRequest InsertCalendarevent(GoogleCalendarEvent eventToInsert)
    {
        string stringEventToInsert = JsonUtility.ToJson(eventToInsert);
        UnityWebRequest post = new UnityWebRequest(credentials.calendar_endpoint + credentials.calendar_id + "/events?access_token=" + authentificator.gat.access_token + "&t=" + getUTCTime(), "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(stringEventToInsert);
        post.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        post.SetRequestHeader("Content-Type", "application/json");
        post.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        return post;
    }

    void ReadGoogleCalendarCredentials()
    {
        TextAsset txtAsset = (TextAsset)Resources.Load("Credentials/credentials_googlecalendar_workAR", typeof(TextAsset));
        credentials = JsonUtility.FromJson<GoogleCrendentials>(txtAsset.text);
    }

    string getUTCTime()
    {
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp.ToString();
    }
}
