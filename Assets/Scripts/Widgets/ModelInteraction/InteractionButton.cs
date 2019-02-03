using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;

public interface IInteractionReceiver
{
   void TypeGotActivated(ManipulationMode mode);
}

public class InteractionButton : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    ManipulationMode mode;

    public IInteractionReceiver receiver;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        receiver.TypeGotActivated(mode);
    }
}