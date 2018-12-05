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

    void OnEnable()
    {
        ErrorField.Instance.textMesh.text = "Awake happened: authentificator?" + (authentificator != null) +"\n" + ErrorField.Instance.textMesh.text;
        ErrorField.Instance.textMesh.text = "OnEnable: creating google authentificator\n" + ErrorField.Instance.textMesh.text;
        Application.logMessageReceived += handleUnityLog;
        authentificator = new GoogleAuthentification();
        ReadGoogleCalendarCredentials();
    }

    private void handleUnityLog(string logString, string stackTrace, LogType type)
    {
        ErrorField.Instance.textMesh.text = "Trace: " + logString + "\n" + ErrorField.Instance.textMesh.text;
        ErrorField.Instance.textMesh.text = "StackTrace: " + stackTrace.ToString() + "\n" + ErrorField.Instance.textMesh.text;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= handleUnityLog;
    }

    void Start()
    {
        ErrorField.Instance.textMesh.text = "Awake happened: authentificator?" + (authentificator != null) + "\n" + ErrorField.Instance.textMesh.text;
        ErrorField.Instance.textMesh.text = "Start: creating google authentificator\n" + ErrorField.Instance.textMesh.text;
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
        ErrorField.Instance.textMesh.text = "get calendar events\n" + ErrorField.Instance.textMesh.text;
        ErrorField.Instance.textMesh.text = "is authentificator available?" + (authentificator != null) + "\n" + ErrorField.Instance.textMesh.text;
        ErrorField.Instance.textMesh.text = "is credentials available?" + (credentials.calendar_id != null) + "\n" + ErrorField.Instance.textMesh.text;
        ErrorField.Instance.textMesh.text = "is gat available?" + (authentificator.gat != null) +"\n" + ErrorField.Instance.textMesh.text;
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
        ErrorField.Instance.textMesh.text = "read google credentials\n" + ErrorField.Instance.textMesh.text;
        TextAsset txtAsset = (TextAsset)Resources.Load("Credentials/credentials_googlecalendar_workAR", typeof(TextAsset));
        credentials = JsonUtility.FromJson<GoogleCrendentials>(txtAsset.text);
    }

    string getUTCTime()
    {
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp.ToString();
    }
}
