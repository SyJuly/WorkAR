using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardColumnSortModifier : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    float positionY = 0.42f;

    [SerializeField]
    float scaleY = 0.16f;

    [SerializeField]
    GameObject buttonIndicator;

    [SerializeField]
    public Renderer meshRenderer;

    public BoardColumn boardColumn;

    private Vector3 previousPosition;
    private Vector3 previousScale;
    private Transform boardColumnCube;

    public Sorter sorter;

    private void Awake()
    {
        boardColumn = GetComponent<BoardColumn>();
        boardColumnCube = meshRenderer.transform;
    }

    void OnEnable()
    {
        buttonIndicator.SetActive(true);
    }

    void OnDisable()
    {
        buttonIndicator.SetActive(false);
    }

    public void ActivateSortMode()
    {
        boardColumn.dictationNoteColumn.SetActive(false);
        previousPosition = new Vector3(boardColumnCube.localPosition.x, boardColumnCube.localPosition.y, boardColumnCube.localPosition.z);
        previousScale = new Vector3(boardColumnCube.localScale.x, boardColumnCube.localScale.y, boardColumnCube.localScale.z);

        boardColumnCube.localPosition = new Vector3(boardColumnCube.localPosition.x, positionY, boardColumnCube.localPosition.z);
        boardColumnCube.localScale = new Vector3(boardColumnCube.localScale.x, scaleY, boardColumnCube.localScale.z);
    }

    public void DeactivateSortMode()
    {
        boardColumn.dictationNoteColumn.SetActive(true);
        boardColumnCube.localPosition = previousPosition;
        boardColumnCube.localScale = previousScale;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        sorter.ClickedList(this);
    }
}
