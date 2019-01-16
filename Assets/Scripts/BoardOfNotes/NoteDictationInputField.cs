using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDictationInputField : DictationInputField, IConfirmButton, ICancelButton
{

    [SerializeField]
    InputNote inputNotePrefab;
    
    PhotoCaptureWithHolograms capturer;

    public string idList;

    private InputNote inputNote;

    string lastMessage;

    protected override void Awake()
    {
        base.Awake();
        reactingObject = GetComponent<ContentCreationButton>();
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
        inputNote = Instantiate(inputNotePrefab);
        inputNote.confirmButton.receiver = this;
        inputNote.cancelButton.receiver = this;
        capturer = inputNote.capturer;
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
        StopInputNote();
    }

    public void OnCancel()
    {
        Debug.Log("cancel button pressed: CANCEL");
        StopInputNote();
    }

    private void StopInputNote()
    {
        inputNote.CompleteInput();
        inputNote = null;
    }
}
