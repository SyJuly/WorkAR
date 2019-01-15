using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionModelLoadButton : MonoBehaviour, IInputClickHandler, IFocusable
{
    Placeable placable;

    private bool isFocused;

    private bool isImporting;

    void Start()
    {
        placable = GetComponent<Placeable>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!eventData.used && isFocused && !placable.IsPlacing)
        {
            eventData.Use();
            InteractionModellLoader.Instance.Get3DModell();
        }
    }

    public void OnFocusEnter()
    {
        isFocused = true;
    }

    public void OnFocusExit()
    {
        isFocused = false;
    }
}
