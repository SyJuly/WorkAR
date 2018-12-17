using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICancelButton
{
    void OnCancel();
}

public class CancelButton : MonoBehaviour, IInputClickHandler
{
    public ICancelButton receiver;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        receiver.OnCancel();
    }
}