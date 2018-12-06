using System;

[Serializable]
public struct TrelloCards
{
    public TrelloCard[] cards;
}

[Serializable]
public struct TrelloCard
{
    public string idList;
    public string name;
    public string pos;
    public string dueComplete;
}

