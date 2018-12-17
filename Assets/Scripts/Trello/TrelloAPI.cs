using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TrelloAPI : MonoBehaviour {

    TrelloCrendentials credentials;

    /*------------------Singleton---------------------->>*/
    private static TrelloAPI _instance;

    public static TrelloAPI Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        ReadTrelloCredentials();
    }
    /*<<------------------Singleton-----------------------*/

    public UnityWebRequest GetBoardTitleHTTPRequest()
    {
        return UnityWebRequest.Get(credentials.trello_board_endpoint + credentials.board_id + "?token=" + credentials.access_token + "&key=" + credentials.api_key +"&t=" + getUTCTime());
    }

    public UnityWebRequest GetBoardListsHTTPRequest()
    {
        return UnityWebRequest.Get(credentials.trello_board_endpoint + credentials.board_id + "/lists?token=" + credentials.access_token + "&key=" + credentials.api_key + "&t=" + getUTCTime());
    }

    public UnityWebRequest GetAllCardsHTTPRequest()
    {
        return UnityWebRequest.Get(credentials.trello_board_endpoint + credentials.board_id + "/cards?token=" + credentials.access_token + "&key=" + credentials.api_key + "&t=" + getUTCTime());
    }

    public UnityWebRequest InsertCard(TrelloCard cardToInsert)
    {
        UnityWebRequest post = new UnityWebRequest(credentials.trello_card_endpoint + "?token=" + credentials.access_token + "&key=" + credentials.api_key + "&t=" + getUTCTime()
            + "&idList=" + cardToInsert.idList
            + "&name=" + cardToInsert.name, "POST");
        return post;
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
