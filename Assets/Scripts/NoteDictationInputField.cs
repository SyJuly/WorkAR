using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDictationInputField : DictationInputField
{
    WriteToTrello trelloWriter;

    public string idList;

    protected override void Start()
    {
        base.Start();
        trelloWriter = GetComponentInParent<WriteToTrello>();
        reactingObject = GetComponent<EventCreationButton>();
    }

    public override void ReceiveDictationResult(string message)
    {
        reactingObject.ReactOnDictationStop();
        TrelloCard createdCard = new TrelloCard(idList, message, "bottom");
        trelloWriter.SendCardToTrello(createdCard);
    }
}
