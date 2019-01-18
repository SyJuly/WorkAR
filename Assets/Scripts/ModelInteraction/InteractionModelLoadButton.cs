using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class InteractionModelLoadButton : MonoBehaviour, IInputClickHandler, IFocusable
{
    private Placeable placable;

    private InteractionModelLoader modelLoader;

    private bool isFocused;

    private bool isImporting;

    void Start()
    {
        placable = GetComponent<Placeable>();
        modelLoader = GetComponent<InteractionModelLoader>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!eventData.used && isFocused && !placable.IsPlacing)
        {
            eventData.Use();
            modelLoader.Get3DModel();
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
