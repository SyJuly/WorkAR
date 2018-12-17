using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public Vector3 point;

    NoteDictationInputField noteInputField;

    private void Awake()
    {
        noteInputField = GetComponent<NoteDictationInputField>();
    }

    private void Start()
    {
        noteInputField.OnInputClicked(new InputClickedEventData(UnityEngine.EventSystems.EventSystem.current));
    }
}
