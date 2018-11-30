using HoloToolkit.Unity.InputModule;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class DictationInputField : MonoBehaviour, IInputClickHandler, IFocusable
{
    WriteToGoogleCalendar googleCalendarWriter;

    DictationRecognizer dictationRecognizer;

    [SerializeField]
    TextMeshPro eventTextField;

    bool dictateModeOn = false;

    bool isFocussed = false;

    void Start () {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        googleCalendarWriter = GetComponentInParent<WriteToGoogleCalendar>();
        Debug.Log("dictation recognizer instantiated.");
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("DictationResult: " + text);
        eventTextField.text = text;
        googleCalendarWriter.SendEventToCalendar(text);
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("Completeing dictation: " + cause);
        dictationRecognizer.Stop();
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.Log("Error during dictation: " + error);
        dictationRecognizer.Stop();
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
            Debug.Log("input clicked, let's start Dictation");
            dictationRecognizer.Start();
        }
    }
}
