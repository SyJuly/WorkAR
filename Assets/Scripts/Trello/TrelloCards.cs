using System;
using UnityEngine;

[Serializable]
public struct TrelloCards
{
    public TrelloCard[] cards;
}

[Serializable]
public class TrelloCard
{
    public TrelloCard(string idList, string name, string pos)
    {
        this.idList = idList;
        this.name = name;
        this.pos = pos;
    }
    public TrelloCard(string idList, string name, string pos, Texture2D attachment)
    {
        this.idList = idList;
        this.name = name;
        this.pos = pos;
        this.attachment = attachment;
    }
    public string idList;
    public string name;
    public string pos;
    public string id;
    public string idAttachmentCover;
    public Texture2D attachment;
}

