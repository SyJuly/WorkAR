using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSortModifier : MonoBehaviour, IInputClickHandler
{
    public Renderer meshRenderer;

    public Sorter sorter;

    public Note note;

    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        note = GetComponent<Note>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        sorter.ClickedNote(this);
    }
}
