using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICancelButton
{
    void OnCancel();
}

public class CancelButton : MonoBehaviour, IInputClickHandler, IFocusable
{
    public ICancelButton receiver;

    private bool isFocussed = false;

    public void OnFocusEnter()
    {
        isFocussed = true;
    }

    public void OnFocusExit()
    {
        isFocussed = false;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (isFocussed)
        {
            receiver.OnCancel();
        }
    }
}