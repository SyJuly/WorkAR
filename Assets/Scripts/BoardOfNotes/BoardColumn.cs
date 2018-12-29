using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardColumn : MonoBehaviour {

    public TrelloList list;
    
    TextMeshPro listTitleTextField;

    Bounds bounds;

    [SerializeField]
    GameObject notePrefab;

    [SerializeField]
    GameObject noteParent;

    [SerializeField]
    int maxNumberOfNotes = 4;

    public bool isUsed = false;

    TrelloCard[] currentCards;

    Dictionary<string, Note> createdNotes;

    private void Awake()
    {
        bounds = noteParent.GetComponent<MeshFilter>().mesh.bounds;
        listTitleTextField = GetComponentInChildren<TextMeshPro>();
        currentCards = new TrelloCard[0];
        createdNotes = new Dictionary<string, Note>();
    }

    public void PlaceNotes()
    {
        listTitleTextField.text = list.name;
        GetComponentInChildren<NoteDictationInputField>().idList = list.id;
        float x = bounds.size.x;
        float y = bounds.size.y;

        float divX = x * 2;
        float divY = y / maxNumberOfNotes;
        float topAlign = y / 2;

        float divCounterY = 0;
        for (int n = 0; n < currentCards.Length; n++)
        {
            GameObject note = GetCardNote(currentCards[n]);
            note.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = currentCards[n].name;

            if(currentCards[n].attachment != null)
            {
                Renderer quadRenderer = note.GetComponent<Note>().photoObject.GetComponent<Renderer>() as Renderer;
                quadRenderer.material = new Material(Shader.Find("UI/Default"));
                quadRenderer.material.SetTexture("_MainTex", currentCards[n].attachment);
            }
            float noteY = gameObject.transform.localPosition.y - divCounterY + topAlign - divY / 2;
            float noteX = 0;
            note.transform.localPosition = new Vector3(noteX, noteY, -0.5f);
            note.transform.localRotation = Quaternion.identity;
            note.transform.localScale = new Vector3(notePrefab.transform.localScale.x, notePrefab.transform.localScale.y, 0.5f);

            divCounterY += divY;

        }
        CleanupColumn();
    }

    public void UpdateCards(TrelloList updatedList)
    {
        list = updatedList;
        Dictionary<string, TrelloCard> cardsById = new Dictionary<string, TrelloCard>();
        foreach (TrelloCard card in updatedList.cards)
        {
            cardsById.Add(card.id, card);
        }
        
        TrelloCard[] newCards = new TrelloCard[cardsById.Count];
        int i = 0;
        foreach (KeyValuePair<string, TrelloCard> cardById in cardsById)
        {
            newCards[i] = cardById.Value;
            i++;
        }
        foreach (KeyValuePair<string, Note> note in createdNotes)
        {
            note.Value.isUsed = false;
        }
        currentCards = newCards;
        PlaceNotes();
    }

    private GameObject GetCardNote(TrelloCard card)
    {
        Note note = null;
        if (createdNotes.ContainsKey(card.id))
        {
            note = createdNotes[card.id];
        }
        else
        {
            GameObject noteGO = Instantiate(notePrefab, noteParent.transform);
            Note newNote = noteGO.GetComponentInChildren<Note>();
            createdNotes.Add(card.id, newNote);
            note = newNote;
        }
        note.isUsed = true;
        return note.gameObject;
    }

    List<string> keyToDestroy = new List<string>();

    private void CleanupColumn()
    {
        keyToDestroy.Clear();
        foreach (KeyValuePair<string, Note> note in createdNotes)
        {
            if (note.Value.isUsed == false)
            {
                keyToDestroy.Add(note.Key);
            }
        }
        foreach (string key in keyToDestroy)
        {
            Destroy(createdNotes[key].gameObject);
            createdNotes.Remove(key);
        }
    }
}
