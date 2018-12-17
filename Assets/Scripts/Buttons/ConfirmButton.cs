using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConfirmButton
{
    void OnConfirm();
}

public class ConfirmButton : MonoBehaviour, IInputClickHandler {

    public IConfirmButton receiver;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        receiver.OnConfirm();
    }
}
