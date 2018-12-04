using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventCreationButton : MonoBehaviour, IReactOnDictationInput
{
    Image buttonImage;

    [SerializeField]
    Sprite creating;

    [SerializeField]
    Sprite recording;

    TextMeshProUGUI buttonTextField;

    void Start() {
        buttonTextField = GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = GetComponentInChildren<Image>();
    }

    public void ReactOnDictationStart()
    {
        buttonTextField.text = "Recording";
        buttonImage.sprite = recording;
    }

    public void ReactOnDictationStop()
    {
        buttonTextField.text = "Create Event";
        buttonImage.sprite = creating;
    }
}
