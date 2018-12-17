using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDictationInputField : DictationInputField, IConfirmButton, ICancelButton
{
    WriteToTrello trelloWriter;

    [SerializeField]
    InputNote inputNotePrefab;

    public string idList;

    private InputNote inputNote;

    string lastMessage;

    protected override void Awake()
    {
        base.Awake();
        trelloWriter = GetComponentInParent<WriteToTrello>();
        reactingObject = GetComponent<ContentCreationButton>();
    }

    public override void ReceiveDictationResult(string message)
    {
        reactingObject.ReactOnDictationStop();
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
    }
    public void OnConfirm()
    {
        Debug.Log("confirm button pressed: CONFIRM");
        TrelloCard createdCard = new TrelloCard(idList, lastMessage, "bottom");
        trelloWriter.SendCardToTrello(createdCard);
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
