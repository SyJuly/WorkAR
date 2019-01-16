using AsImpL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadModelFromTrello
{
    TrelloAPI trelloAPI;

    public LoadModelFromTrello(TrelloAPI api)
    {
        trelloAPI = api;
    }

    public void Get3DModel(System.Action<string> urlAction)
    {
        Utility.Instance.StartCoroutine(GetModellCard(urlAction));
    }

    IEnumerator GetModellCard(System.Action<string> urlAction)
    {
        UnityWebRequest ModellCardRequest = trelloAPI.GetModellCardHTTPRequest();
        ModellCardRequest.chunkedTransfer = false;
        ModellCardRequest.timeout = 100000;

        yield return ModellCardRequest.SendWebRequest();
        if (ModellCardRequest.isNetworkError || ModellCardRequest.isHttpError)
        {
            Debug.Log("An error occured receiving modell card: " + ModellCardRequest.responseCode);
        }
        else
        {
            string responseToJSON = "{\"card\":" + ModellCardRequest.downloadHandler.text + "}";
            TrelloCard card = JsonUtility.FromJson<TrelloCard>(responseToJSON);
            yield return Utility.Instance.StartCoroutine(GetModellLinkAttachment(card, urlAction));
        }
    }

    IEnumerator GetModellLinkAttachment(TrelloCard card, System.Action<string> urlAction)
    {
        UnityWebRequest ModellLinkAttachmentRequest = trelloAPI.GetLinkAttachmentHTTPRequest();
        ModellLinkAttachmentRequest.chunkedTransfer = false;
        ModellLinkAttachmentRequest.timeout = 100000;

        yield return ModellLinkAttachmentRequest.SendWebRequest();
        if (ModellLinkAttachmentRequest.isNetworkError || ModellLinkAttachmentRequest.isHttpError)
        {
            Debug.Log("An error occured receiving modell attachment link: " + ModellLinkAttachmentRequest.responseCode);
        }
        else
        {
            string responseToJSON = "{\"trelloAttachments\":" + ModellLinkAttachmentRequest.downloadHandler.text + "}";
            TrelloCardAttachmentsResponse attachments = JsonUtility.FromJson<TrelloCardAttachmentsResponse>(responseToJSON);
            Debug.Log("attachment link? " + attachments.trelloAttachments[0].url);
            string url = attachments.trelloAttachments[0].url;
            urlAction?.Invoke(url);
        }
    }
}
