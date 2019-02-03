using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInputNote : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    GameObject inputNotePrefab;

    [SerializeField]
    private float minTimeDiffToSetMarkerInSec = 1f;

    public string idList;

    private InputNote currentInputNote;

    private float lastTimeOpenedInputNote = 0f;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if(Time.time - lastTimeOpenedInputNote > minTimeDiffToSetMarkerInSec)
        {
            lastTimeOpenedInputNote = Time.time;
            GameObject inputNoteGO = Instantiate(inputNotePrefab);
            currentInputNote = inputNoteGO.GetComponent<InputNote>();
            NoteDictationInputField inputField = inputNoteGO.GetComponentInChildren<NoteDictationInputField>();
            inputField.idList = idList;
            inputField.opener = this;
            currentInputNote.confirmButton.receiver = inputField;
            currentInputNote.cancelButton.receiver = inputField;
            inputField.capturer = currentInputNote.capturer;
        }
    }

    public void StopInputNote()
    {
        currentInputNote.CompleteInput();
        currentInputNote = null;
    }
}
