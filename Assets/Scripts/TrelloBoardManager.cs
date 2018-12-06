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

    BoardColumn[] lastCreatedColumns;

    private void Start()
    {
        bounds = GetComponent<MeshFilter>().mesh.bounds;
        currentListsWithCards = new TrelloList[0];
        lastCreatedColumns = new BoardColumn[0];
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

            float fieldY = gameObject.transform.localPosition.y - borderTop;

            GameObject field = Instantiate(fieldPrefab, transform);
            BoardColumn boardColumnField = field.GetComponent<BoardColumn>();
            lastCreatedColumns[n] = boardColumnField;
            boardColumnField.list = currentListsWithCards[n];
            float fieldX = gameObject.transform.localPosition.x + divCounterX - leftAlign - divX / 2;
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
        CleanupBoardColumns(cardsByList.Count);
        int i = 0;
        foreach (KeyValuePair<string, TrelloList> cardByList in cardsByList)
        {
            newListsWithCards[i] = cardByList.Value;
            i++;
        }
        currentListsWithCards = newListsWithCards;
        PlaceBoardColumns();
    }

    private void CleanupBoardColumns(int newNumberOfBoardColumns)
    {
        for(int i = 0; i < lastCreatedColumns.Length; i++)
        {
            Destroy(lastCreatedColumns[i].gameObject);
        }
        lastCreatedColumns = new BoardColumn[newNumberOfBoardColumns];
    }
}
