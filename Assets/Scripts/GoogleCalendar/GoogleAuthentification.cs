using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System;


// for implementation of asking user for authentification
public class GoogleAuthentification : MonoBehaviour
{
    GoogleCrendentials credentials;

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
        StartCoroutine(GetOAuthToken_TempToken_AllEvents());
    }

    IEnumerator GetOAuthToken_TempToken_AllEvents()
    {
        UnityWebRequest AuthentificationCodeRequest = UnityWebRequest.Get(credentials.auth_uri + "?client_id=" + credentials.client_id + "&access_type=offline&redirect_uri=" + credentials.redirect_uri + "&scope=https://www.googleapis.com/auth/calendar.events&response_type=code");
        yield return AuthentificationCodeRequest.SendWebRequest();
        if (AuthentificationCodeRequest.isNetworkError || AuthentificationCodeRequest.isHttpError)
        {
            Debug.Log(AuthentificationCodeRequest.error);
        }
        else
        {
            string response = AuthentificationCodeRequest.downloadHandler.text;

            //TODO: Show user authentification window to accept

            //StartCoroutine(GetAccessToken(authentificationCode));
        }
    }

    IEnumerator GetAccessToken(String authentificationCode)
    {
        List<IMultipartFormSection> parameters = new List<IMultipartFormSection>();
        parameters.Add(new MultipartFormDataSection("client_secret", credentials.client_secret));
        parameters.Add(new MultipartFormDataSection("grant_type", "authorization_code"));
        parameters.Add(new MultipartFormDataSection("code", authentificationCode));
        parameters.Add(new MultipartFormDataSection("redirect_uri", credentials.redirect_uri));
        parameters.Add(new MultipartFormDataSection("client_id", credentials.client_id));

        UnityWebRequest wwwPost = UnityWebRequest.Post(credentials.token_uri, parameters);
        yield return wwwPost.SendWebRequest();

        if (wwwPost.isNetworkError || wwwPost.isHttpError)
        {
            Debug.Log(wwwPost.error);
        }
        else
        {
            string response = wwwPost.downloadHandler.text;

            //Access_Token_Response atr = JsonUtility.FromJson<Access_Token_Response>(response);

            SaveAccessToken(response);
        }
    }

    void SaveAccessToken(String accesstoken)
    {
        string path = "Credentials/access_token.json";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(accesstoken);
        writer.Close();
    }
}
