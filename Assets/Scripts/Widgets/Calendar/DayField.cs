using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DayField : MonoBehaviour
{
    public DateTime representedDay;

    Calendar calendar;

    [SerializeField]
    TextMeshProUGUI dayNumberTextField;

    EventLine eventLine;

    bool isCreating = false;

    int numberOfEventsShowing = 0;

    void Start()
    {
        calendar = GetComponentInParent<Calendar>();
        eventLine = GetComponentInChildren<EventLine>();
        eventLine.gameObject.SetActive(false);
        dayNumberTextField.text = representedDay.Day.ToString();
    }

    public void UpdateDayEvents(GoogleCalendarEvent[] events)
    {
        if (events != null)
        {
            foreach (GoogleCalendarEvent calendarEvent in events)
            {
                if(calendarEvent.start.dateTime == null)
                {
                    return;
                }
                DateTime startTime = DateTime.ParseExact(calendarEvent.start.dateTime, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
                if (startTime.Year == representedDay.Year
                    && startTime.Month == representedDay.Month
                    && startTime.Day == representedDay.Day
                    && numberOfEventsShowing < 1
                    && !isCreating)
                {
                    eventLine.gameObject.SetActive(true);
                    numberOfEventsShowing++;
                    eventLine.eventTitleTextField.text = calendarEvent.summary;
                    DateTime endTime = DateTime.ParseExact(calendarEvent.end.dateTime, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
                    eventLine.eventTimeTextField.text = startTime.ToString("HH:mm") + "\n" + endTime.ToString("HH:mm");
                }
            }
        }

    }

    public void CreatingEvent(string message, int hourAllEventsBegin, int hourAllEventsEnd)
    {
        if (!isCreating)
        {
            eventLine.gameObject.SetActive(true);
            DateTime start = new DateTime(representedDay.Year, representedDay.Month, representedDay.Day, hourAllEventsBegin, 0, 0);
            DateTime end = new DateTime(representedDay.Year, representedDay.Month, representedDay.Day, hourAllEventsEnd, 0, 0);
            eventLine.eventTimeTextField.text = start.ToString("HH:mm") + "\n" + end.ToString("HH:mm");
        }
        isCreating = true;
        eventLine.eventTitleTextField.text = message + "|";
    }

    public void StopCreatingEvent()
    {
        isCreating = false;
    }
}
