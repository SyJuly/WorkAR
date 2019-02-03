using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDictationInputField : DictationInputField, IConfirmButton, ICancelButton
{
    public PhotoCaptureWithHolograms capturer;

    [SerializeField]
    GameObject dictationButton;

    public string idList;

    public InputNote inputNote;

    string lastMessage;

    public OpenInputNote opener;

    protected override void Awake()
    {
        base.Awake();
        reactingObject = GetComponent<ContentCreationButton>();
        inputNote = GetComponentInParent<InputNote>();
    }

    public override void ReceiveDictationResult(string message)
    {
        if(reactingObject != null)
        {
            reactingObject.ReactOnDictationStop();
        }
        lastMessage = message;
    }

    public override void ReceiveDictationHypothesis(string message)
    {
        lastMessage = message;
        inputNote.SendHypothesis(message);
    }

    public override void ReceiveDictationStart()
    {
        dictationButton.SetActive(false);
    }

    public void OnConfirm()
    {
        Debug.Log("confirm button pressed: CONFIRM");
        TrelloCard createdCard;
        if (capturer.isPhotoReadyToSend)
        {
            createdCard = new TrelloCard(idList, lastMessage, "bottom", capturer.targetTexture);
            capturer.isPhotoReadyToSend = false;
        } else
        {
            createdCard = new TrelloCard(idList, lastMessage, "bottom");
        }
        WebManager.Instance.Trello.Writer.SendCardToTrello(createdCard);
        opener.StopInputNote();
    }

    public void OnCancel()
    {
        opener.StopInputNote();
    }
}
