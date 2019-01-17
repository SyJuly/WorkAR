using System;
using System.Collections.Generic;

[Serializable]
public struct TrelloLists
{
    public TrelloList[] lists;
}

[Serializable]
public struct TrelloList
{
    public string id;
    public string name;
    public string pos;
    public List<TrelloCard> cards;
}

