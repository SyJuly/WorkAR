using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrelloAPI : MonoBehaviour {

    TrelloCrendentials credentials;

    void Awake()
    {
        ReadTrelloCredentials();
    }

    public UnityWebRequest GetBoardTitleHTTPRequest()
    {
        return UnityWebRequest.Get(credentials.trello_endpoint + credentials.board_id + "?token=" + credentials.access_token + "&key=" + credentials.api_key +"&t=" + getUTCTime());
    }

    public UnityWebRequest GetBoardListsHTTPRequest()
    {
        return UnityWebRequest.Get(credentials.trello_endpoint + credentials.board_id + "/lists?token=" + credentials.access_token + "&key=" + credentials.api_key + "&t=" + getUTCTime());
    }

    public UnityWebRequest GetAllCardsHTTPRequest()
    {
        return UnityWebRequest.Get(credentials.trello_endpoint + credentials.board_id + "/cards?token=" + credentials.access_token + "&key=" + credentials.api_key + "&t=" + getUTCTime());
    }

    void ReadTrelloCredentials()
    {
        TextAsset txtAsset = (TextAsset)Resources.Load("Credentials/credentials_trello_workAR", typeof(TextAsset));
        credentials = JsonUtility.FromJson<TrelloCrendentials>(txtAsset.text);
    }

    string getUTCTime()
    {
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp.ToString();
    }
}
