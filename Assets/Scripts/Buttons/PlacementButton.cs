using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementButton : MonoBehaviour, IFocusable
{
    public Placeable source;

    public void OnFocusEnter()
    {
        source.IsFocused(true);
    }

    public void OnFocusExit()
    {
        source.IsFocused(false);
    }
}
