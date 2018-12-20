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

    [SerializeField]
    InteractionModel model;

    CursorFeedback cursorfeedback;

    ManipulationMode activeInteractionType = ManipulationMode.Scale;

    bool isShowingFeedback;

    bool changedManipulationMode;

    void Start()
    {
        move.receiver = this;
        rotate.receiver = this;
        scale.receiver = this;
        TypeGotActivated(activeInteractionType);
        cursorfeedback = CursorFeedback.Instance.gameObject.GetComponent<CursorFeedback>();
    }

    void Update()
    {
        if(!isShowingFeedback && model.isFocused)
        {
            cursorfeedback.ActivateManipulationModeFeedback(activeInteractionType);
            isShowingFeedback = true;
        } else if (!model.isFocused && isShowingFeedback)
        {
            cursorfeedback.ActivateManipulationModeFeedback(ManipulationMode.None);
            isShowingFeedback = false;
        }
    }

    public void TypeGotActivated(ManipulationMode mode)
    {
        activeInteractionType = mode;
        adjustManipulationMode();
        if (isShowingFeedback)
        {
            cursorfeedback.ActivateManipulationModeFeedback(mode);
        }
    }

    private void adjustManipulationMode()
    {
        twoHandManipulatable.ManipulationMode = activeInteractionType;
    }
}
