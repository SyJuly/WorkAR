using AsImpL;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class InteractionModellLoader : ObjectImporter, IInputClickHandler
{

    TrelloAPI trelloAPI;

    [SerializeField]
    GameObject interactionModelPrefab;

    InteractionModel interactionModelParent;

    void Start()
    {
        trelloAPI = TrelloAPI.Instance.gameObject.GetComponent<TrelloAPI>();
    }

    void Get3DModell()
    {
        StartCoroutine(GetModellCard());
    }

    IEnumerator GetModellCard()
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
            yield return StartCoroutine(GetModellLinkAttachment(card));
        }
    }

    IEnumerator GetModellLinkAttachment(TrelloCard card)
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
            yield return StartCoroutine(ImportObjFromUrl(url));
        }
    }

    IEnumerator ImportObjFromUrl(string url)
    {
        ObjectImporter objectLoadedImporter = this;
        ImportOptions options = new ImportOptions();
        options.reuseLoaded = true;
        options.inheritLayer = false;
        options.modelScaling = 0.3f;
        options.localPosition = Vector3.zero;
        Debug.Log("start import model async");
        objectLoadedImporter.ImportModelAsync("model", url, interactionModelParent.transform, options);
        TranslateModelInFrontOfWidget();
        yield return null;
    }

    void TranslateModelInFrontOfWidget()
    {
        transform.Translate(1,1,1, Camera.main.transform);
    }

    void ActivateSingleCollider()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Collider colliderToDeactivate = meshFilters[i].GetComponent<Collider>();
            if (colliderToDeactivate)
            {
                colliderToDeactivate.enabled = false;
            }

            i++;
        }
        MeshFilter filter = transform.GetComponent<MeshFilter>();
        filter.mesh = new Mesh();
        filter.mesh.CombineMeshes(combine);
        MeshCollider collider = transform.GetComponent<MeshCollider>();
        collider.enabled = true;
        collider.sharedMesh = filter.mesh;

    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("on input clicked");
        GameObject model = Instantiate(interactionModelPrefab);
        interactionModelParent = model.GetComponentInChildren<InteractionModel>();
        Get3DModell();
    }
}
