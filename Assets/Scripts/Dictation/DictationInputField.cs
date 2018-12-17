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

public abstract class DictationInputField : MonoBehaviour, IInputClickHandler, IDictationInputReceiver
{
    DictationInputHandler dictationHandler;

    protected IReactOnDictationInput reactingObject;

    protected virtual void Awake () {
        dictationHandler = DictationInputHandler.Instance.gameObject.GetComponent<DictationInputHandler>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        dictationHandler.ActivateInputField(this);
        if (reactingObject != null)
        {
            reactingObject.ReactOnDictationStart();
        }
        
    }

    public abstract void ReceiveDictationResult(string message);

    public abstract void ReceiveDictationHypothesis(string message);

    public abstract void ReceiveDictationStart();
}
