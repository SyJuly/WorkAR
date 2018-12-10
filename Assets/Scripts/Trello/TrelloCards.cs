using System;

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
    public string idList;
    public string name;
    public string pos;
    //public string dueComplete;
}

