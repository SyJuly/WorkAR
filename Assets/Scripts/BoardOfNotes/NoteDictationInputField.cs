using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDictationInputField : DictationInputField
{
    WriteToTrello trelloWriter;

    [SerializeField]
    InputNote inputNotePrefab;

    public string idList;

    private InputNote inputNote;

    protected override void Start()
    {
        base.Start();
        trelloWriter = GetComponentInParent<WriteToTrello>();
        reactingObject = GetComponent<ContentCreationButton>();
    }

    public override void ReceiveDictationResult(string message)
    {
        reactingObject.ReactOnDictationStop();
        inputNote.CompleteInput();
        inputNote = null;
        TrelloCard createdCard = new TrelloCard(idList, message, "bottom");
        trelloWriter.SendCardToTrello(createdCard);
    }

    public override void ReceiveDictationHypothesis(string message)
    {
        inputNote.SendHypothesis(message);
    }

    public override void ReceiveDictationStart()
    {
        inputNote = Instantiate(inputNotePrefab);
        inputNote.transform.localScale = inputNotePrefab.transform.localScale;
    }
}
