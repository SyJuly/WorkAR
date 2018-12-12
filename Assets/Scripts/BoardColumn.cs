﻿using System.Collections;
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

    private void Start()
    {
        bounds = noteParent.GetComponent<MeshFilter>().mesh.bounds;
        listTitleTextField = GetComponentInChildren<TextMeshPro>();
        listTitleTextField.text = list.name;
        PlaceNotes();
    }

    public void PlaceNotes()
    {
        GetComponentInChildren<NoteDictationInputField>().idList = list.id;
        List<TrelloCard> cards = list.cards;
        float x = bounds.size.x;
        float y = bounds.size.y;

        float divX = x * 2;
        float divY = y / maxNumberOfNotes;
        float topAlign = y / 2;

        float divCounterY = 0;
        for (int n = 0; n < cards.Count; n++)
        {
            GameObject note = Instantiate(notePrefab, noteParent.transform);
            note.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = list.cards[n].name;
            float noteY = gameObject.transform.localPosition.y - divCounterY + topAlign;
            float noteX = 0;
            note.transform.localPosition = new Vector3(noteX, noteY, -0.5f);
            note.transform.localRotation = Quaternion.identity;
            note.transform.localScale = new Vector3(notePrefab.transform.localScale.x, notePrefab.transform.localScale.y, 0.5f);

            divCounterY += divY;

        }
    }
}