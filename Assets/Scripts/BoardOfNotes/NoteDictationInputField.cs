using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDictationInputField : DictationInputField
{
    WriteToTrello trelloWriter;

    [SerializeField]
    Note notePrefab;

    public string idList;

    protected override void Start()
    {
        base.Start();
        trelloWriter = GetComponentInParent<WriteToTrello>();
        reactingObject = GetComponent<ContentCreationButton>();
    }

    public override void ReceiveDictationResult(string message)
    {
        reactingObject.ReactOnDictationStop();
        TrelloCard createdCard = new TrelloCard(idList, message, "bottom");
        trelloWriter.SendCardToTrello(createdCard);
    }

    public override void ReceiveDictationHypothesis(string message)
    {
        //Note note = Instantiate(notePrefab, transform);
        //note.SetToActiveView();
        //throw new System.NotImplementedException();
    }
}
