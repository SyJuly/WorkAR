using HoloToolkit.Unity.InputModule;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class DictationInputHandler : MonoBehaviour
{

    DictationRecognizer dictationRecognizer;

    public DictationInputField activatedInputField;

    void Start()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
    }

    void OnDisable()
    {
        dictationRecognizer.Stop();
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("DictationResult: " + text);
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
}
