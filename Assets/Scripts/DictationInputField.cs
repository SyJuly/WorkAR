using HoloToolkit.Unity.InputModule;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class DictationInputField : MonoBehaviour, IInputClickHandler, IFocusable, IDictationInputReceiver
{
    WriteToGoogleCalendar googleCalendarWriter;

    DictationInputHandler dictationHandler;

    DayField dayField;

    [SerializeField]
    int hourAllEventsBegin = 8;

    [SerializeField]
    int hourAllEventsEnd = 18;

    [SerializeField]
    TextMeshProUGUI eventTitleTextField;

    bool isFocussed = false;

    void Start () {
        dayField = GetComponent<DayField>();
        dictationHandler = DictationInputHandler.Instance.gameObject.GetComponent<DictationInputHandler>();
        googleCalendarWriter = GetComponentInParent<WriteToGoogleCalendar>();
        Debug.Log("dictation recognizer instantiated.");
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
        }
    }

    public void ReceiveDictationResult(string message)
    {
        eventTitleTextField.text = message;
        DateTime start = new DateTime(dayField.representedDay.Year, dayField.representedDay.Month, dayField.representedDay.Day, hourAllEventsBegin, 0, 0); 
        DateTime end = new DateTime(dayField.representedDay.Year, dayField.representedDay.Month, dayField.representedDay.Day, hourAllEventsEnd, 0, 0); 
        GoogleCalendarEvent createdEvent = new GoogleCalendarEvent(message, start, end);
        googleCalendarWriter.SendEventToCalendar(createdEvent);
    }
}
