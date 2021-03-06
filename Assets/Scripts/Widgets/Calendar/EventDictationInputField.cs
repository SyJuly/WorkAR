﻿using System;
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

    protected override void Awake()
    {
        base.Awake();
        dayField = GetComponentInParent<DayField>();
        googleCalendarWriter = WebManager.Instance.Google.Writer;
        reactingObject = GetComponent<ContentCreationButton>();
    }

    public override void ReceiveDictationResult(string message)
    {
        reactingObject.ReactOnDictationStop();
        dayField.StopCreatingEvent();
        DateTime start = new DateTime(dayField.representedDay.Year, dayField.representedDay.Month, dayField.representedDay.Day, hourAllEventsBegin, 0, 0);
        DateTime end = new DateTime(dayField.representedDay.Year, dayField.representedDay.Month, dayField.representedDay.Day, hourAllEventsEnd, 0, 0);
        GoogleCalendarEvent createdEvent = new GoogleCalendarEvent(message, start, end);
        googleCalendarWriter.SendEventToCalendar(createdEvent);
    }

    public override void ReceiveDictationHypothesis(string message)
    {
        dayField.CreatingEvent(message, hourAllEventsBegin, hourAllEventsEnd);
    }

    public override void ReceiveDictationStart(){}
}
