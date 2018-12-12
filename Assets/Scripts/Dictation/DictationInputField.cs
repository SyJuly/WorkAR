using HoloToolkit.Unity.InputModule;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

public interface IReactOnDictationInput
{
    void ReactOnDictationStart();
    void ReactOnDictationStop();
}

public abstract class DictationInputField : MonoBehaviour, IInputClickHandler, IFocusable, IDictationInputReceiver
{
    DictationInputHandler dictationHandler;

    protected IReactOnDictationInput reactingObject;

    bool isFocussed = false;

    protected virtual void Start () {
        dictationHandler = DictationInputHandler.Instance.gameObject.GetComponent<DictationInputHandler>();
    }

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
            dictationHandler.ActivateInputField(this);
            if (reactingObject != null)
            {
                reactingObject.ReactOnDictationStart();
            }
        }
    }

    public abstract void ReceiveDictationResult(string message);

    public abstract void ReceiveDictationHypothesis(string message);
}
