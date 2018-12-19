using HoloToolkit.Unity.InputModule.Utilities.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPanel : MonoBehaviour, IInteractionReceiver
{

    [SerializeField]
    TwoHandManipulatable twoHandManipulatable;

    [SerializeField]
    InteractionButton move;

    [SerializeField]
    InteractionButton rotate;

    [SerializeField]
    InteractionButton scale;

    ManipulationMode activeInteractionType = ManipulationMode.Scale;

    void Start()
    {
        move.receiver = this;
        rotate.receiver = this;
        scale.receiver = this;
        TypeGotActivated(activeInteractionType);
    }

    public void TypeGotActivated(ManipulationMode mode)
    {
        activeInteractionType = mode;
        adjustManipulationMode();
    }

    private void adjustManipulationMode()
    {
        twoHandManipulatable.ManipulationMode = activeInteractionType;
    }
}
