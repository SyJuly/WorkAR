using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public interface IRefreshedTokenRequester
{
    void AfterRefreshedToken();
}

// for implementation of asking user for authentification
public class GoogleAuthentification
{
    public GoogleAccessToken gat { get; private set; }

    public GoogleAuthentification() {
        ReadGoogleCalendarAccessToken();
    }

    public IEnumerator GetNewAccessToken(UnityWebRequest getAccessTokenHTTPRequest, IRefreshedTokenRequester requester)
    {
        UnityWebRequest NewAccessTokenRequest = getAccessTokenHTTPRequest;
        ErrorField.Instance.textMesh.text = "sending new access token request\n" + ErrorField.Instance.textMesh.text;
        NewAccessTokenRequest.chunkedTransfer = false;
        yield return NewAccessTokenRequest.SendWebRequest();
        if (NewAccessTokenRequest.isNetworkError || NewAccessTokenRequest.isHttpError)
        {
            Debug.Log(NewAccessTokenRequest.downloadHandler.text);
            ErrorField.Instance.textMesh.text = "error while getting new access token:" + NewAccessTokenRequest.downloadHandler.text + "\n" + ErrorField.Instance.textMesh.text;
        }
        else
        {
            string refresh_token = gat.refresh_token;
            gat = JsonUtility.FromJson<GoogleAccessToken>(NewAccessTokenRequest.downloadHandler.text);
            ErrorField.Instance.textMesh.text = "got new access token\n" + ErrorField.Instance.textMesh.text;
            gat.refresh_token = refresh_token;
            requester.AfterRefreshedToken();
        }
    }

    void ReadGoogleCalendarAccessToken()
    {
        ErrorField.Instance.textMesh.text = "reading access token\n" + ErrorField.Instance.textMesh.text;
        TextAsset txtAsset = (TextAsset)Resources.Load("Credentials/access_token", typeof(TextAsset));
        ErrorField.Instance.textMesh.text = "reading access token successfully?" + (txtAsset != null) + "\n" + ErrorField.Instance.textMesh.text;
        gat = JsonUtility.FromJson<GoogleAccessToken>(txtAsset.text);
    }
    /*

    [Serializable]
    public class Access_Token_Response
    {
        public String access_token;
        public String expires_in;
        public String refresh_token;
        public String scope;
        public String token_type;
    }

    void ReadGoogleCalendarCredentials()
    {
        String path = "Credentials/credentials_googlecalendar_workAR.json";

        StreamReader reader = new StreamReader(path);
        String fileText = reader.ReadToEnd();
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
            String response = AuthentificationCodeRequest.downloadHandler.text;

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
            String response = wwwPost.downloadHandler.text;

            //Access_Token_Response atr = JsonUtility.FromJson<Access_Token_Response>(response);

            SaveAccessToken(response);
        }
    }

    void SaveAccessToken(String accesstoken)
    {
        String path = "Credentials/access_token.json";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(accesstoken);
        writer.Close();
    }*/
}
