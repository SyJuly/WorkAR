﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteColumnSortModifier : MonoBehaviour
{
    [SerializeField]
    float positionY = 0.42f;

    [SerializeField]
    float scaleY = 0.16f;

    private BoardColumn boardColumn;

    private Vector3 previousPosition;
    private Vector3 previousScale;

    private void Start()
    {
        boardColumn = GetComponentInParent<BoardColumn>();
    }

    public void ActivateSortMode()
    {
        boardColumn.enabled = false;
        boardColumn.dictationNoteColumn.SetActive(false);
        previousPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        previousScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        transform.localPosition = new Vector3(transform.localPosition.x, positionY, transform.localPosition.z); 
        transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
    }

    public void DeactivateSortMode()
    {
        boardColumn.enabled = true;
        boardColumn.dictationNoteColumn.SetActive(true);
        transform.localPosition = previousPosition;
        transform.localScale = previousScale;
    }

}
