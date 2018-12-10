using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrelloBoardManager : MonoBehaviour
{
    Bounds bounds;

    [SerializeField]
    GameObject fieldPrefab;

    [SerializeField]
    float borderLeft = 0.05f;

    [SerializeField]
    float borderTop = 0.25f;

    [SerializeField]
    int numberOfColumns = 6;

    ReadFromTrello trelloReader;

    TrelloList[] currentListsWithCards;

    Dictionary<string, BoardColumn> createdListColumns;

    private void Start()
    {
        bounds = GetComponent<MeshFilter>().mesh.bounds;
        currentListsWithCards = new TrelloList[0];
        createdListColumns = new Dictionary<string, BoardColumn>();
        trelloReader = GetComponentInParent<ReadFromTrello>();
    }

    private void PlaceBoardColumns()
    {
        float x = bounds.size.x;
        float y = bounds.size.y;

        float divX = ((x - borderLeft * 2) / numberOfColumns);
        float divY = ((y - borderTop * 2));
        float leftAlign = ((x - borderLeft * 2) / 2);

        float divCounterX = divX;
        for (int n = 0; n < currentListsWithCards.Length; n++)
        {
            GameObject field = GetBoardColumn(currentListsWithCards[n]);
            float fieldX = gameObject.transform.localPosition.x + divCounterX - leftAlign - divX / 2;
            float fieldY = 0;
            field.transform.localPosition = new Vector3(fieldX, fieldY, -0.5f);
            field.transform.localRotation = Quaternion.identity;
            field.transform.localScale = new Vector3(fieldPrefab.transform.localScale.x, fieldPrefab.transform.localScale.y, 0.5f);

            divCounterX += divX;

        }
    }

    public void UpdateTrelloBoard()
    {
        Dictionary<string, TrelloList> cardsByList = trelloReader.cardsByList;
        TrelloList[] newListsWithCards = new TrelloList[cardsByList.Count];
        int i = 0;
        foreach (KeyValuePair<string, TrelloList> cardByList in cardsByList)
        {
            newListsWithCards[i] = cardByList.Value;
            i++;
        }
        currentListsWithCards = newListsWithCards;
        PlaceBoardColumns();
    }

    private GameObject GetBoardColumn(TrelloList list)
    {
        if (createdListColumns.ContainsKey(list.id))
        {
            return createdListColumns[list.id].gameObject;
        }
        GameObject field = Instantiate(fieldPrefab, transform);
        BoardColumn boardColumnField = field.GetComponentInChildren<BoardColumn>();
        createdListColumns.Add(list.id, boardColumnField);
        boardColumnField.list = list;
        return field;
    }
}
