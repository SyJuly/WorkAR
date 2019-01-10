using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSortModifier : MonoBehaviour, IInputClickHandler
{
    Renderer renderer;

    public Sorter sorter;

    public Note note;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        note = GetComponent<Note>();
    }

    public void SetColumnColor(Material material)
    {
        renderer.material = material;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        sorter.ClickedNote(this);
    }
}
