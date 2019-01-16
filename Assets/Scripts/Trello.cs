using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trello
{
    private TrelloAPI api;
    public WriteToTrello Writer { get; }
    public ReadFromTrello Reader { get; }
    public LoadModelFromTrello ModelLoader { get; }

    public Trello(){
        api = new TrelloAPI();
        Writer = new WriteToTrello(api);
        Reader = new ReadFromTrello(api);
        ModelLoader = new LoadModelFromTrello(api);
    }
}
