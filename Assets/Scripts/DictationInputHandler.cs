using HoloToolkit.Unity.InputModule;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class DictationInputHandler : MonoBehaviour
{
    /*------------------Singleton---------------------->>*/
    private static DictationInputHandler _instance;

    public static DictationInputHandler Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    /*<<------------------Singleton-----------------------*/

    DictationRecognizer dictationRecognizer;

    public IDictationInputReceiver activatedInputField;

    private bool recordingOn = false;

    void Start()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
    }

    public void ActivateInputField(IDictationInputReceiver dictationReceiver)
    {
        Debug.Log("input clicked, let's start Dictation");
        activatedInputField = dictationReceiver;
        StartRecording();
    }

    void OnDisable()
    {
        StopRecording();
        dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
        dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
        dictationRecognizer.Dispose();
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("DictationResult: " + text);
        activatedInputField.ReceiveDictationResult(text);
        StopRecording();
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("Completeing dictation: " + cause);
        StopRecording();
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.Log("Error during dictation: " + error);
        StopRecording();
    }

    private void StopRecording()
    {
        if (recordingOn)
        {
            recordingOn = false;
            dictationRecognizer.Stop();
        }
    }

    private void StartRecording()
    {
        if (!recordingOn)
        {
            recordingOn = true;
            dictationRecognizer.Start();
        }
    }
}
