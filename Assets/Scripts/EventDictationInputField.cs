using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventDictationInputField : DictationInputField
{
    WriteToGoogleCalendar googleCalendarWriter;

    DayField dayField;

    [SerializeField]
    int hourAllEventsBegin = 8;

    [SerializeField]
    int hourAllEventsEnd = 18;

    protected override void Start()
    {
        base.Start();
        dayField = GetComponentInParent<DayField>();
        googleCalendarWriter = GetComponentInParent<WriteToGoogleCalendar>();
        reactingObject = GetComponent<EventCreationButton>();
    }

    public override void ReceiveDictationResult(string message)
    {
        reactingObject.ReactOnDictationStop();
        DateTime start = new DateTime(dayField.representedDay.Year, dayField.representedDay.Month, dayField.representedDay.Day, hourAllEventsBegin, 0, 0);
        DateTime end = new DateTime(dayField.representedDay.Year, dayField.representedDay.Month, dayField.representedDay.Day, hourAllEventsEnd, 0, 0);
        GoogleCalendarEvent createdEvent = new GoogleCalendarEvent(message, start, end);
        googleCalendarWriter.SendEventToCalendar(createdEvent);
    }
}
