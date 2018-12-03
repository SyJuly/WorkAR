using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class EventCreationButton : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    Image buttonImage;

    [SerializeField]
    Sprite creating;

    [SerializeField]
    Texture recording;

    TextMeshProUGUI buttonTextField;

    void Start() {
        buttonTextField = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        buttonTextField.text = "Recording";
        buttonImage.image = recording;
    }
}
