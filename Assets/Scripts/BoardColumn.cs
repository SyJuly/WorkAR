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
    float borderLeft = 0.05f;

    [SerializeField]
    float borderTop = 0.25f;

    private void Start()
    {
        listTitleTextField = GetComponentInChildren<TextMeshPro>();
        bounds = GetComponent<MeshFilter>().mesh.bounds;
        listTitleTextField.text = list.name;
        PlaceNotes();
    }

    private void PlaceNotes()
    {
        List<TrelloCard> cards = list.cards;
        float x = bounds.size.x;
        float y = bounds.size.y;

        float divX = ((x - borderLeft * 2));
        float divY = ((y - borderTop) / 4);
        float leftAlign = ((x - borderLeft * 2) / 2);
        float topAlign = ((y - borderTop) / 2);

        float divCounterY = 0;
        for (int n = 0; n < cards.Count; n++)
        {
            GameObject note = Instantiate(notePrefab, transform);
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
