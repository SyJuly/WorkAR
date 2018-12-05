using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DayField : MonoBehaviour
{
    
    public DateTime representedDay;

    ReadFromGoogleCalendar googleCalendarReader;

    [SerializeField]
    TextMeshProUGUI dayNumberTextField;

    [SerializeField]
    EventLine eventLinePrefab;

    int numberOfEventsShowing = 0;

    void Start () {
        googleCalendarReader = GetComponentInParent<ReadFromGoogleCalendar>();
    }
	
	void Update () {
        dayNumberTextField.text = representedDay.Day.ToString();
        StartCoroutine(UpdateEvent());
    }

   IEnumerator UpdateEvent()
    {
        if (googleCalendarReader.events != null)
        {
            foreach(GoogleCalendarEvent calendarEvent in googleCalendarReader.events)
            {
                DateTime startTime = DateTime.ParseExact(calendarEvent.start.dateTime, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
                if (startTime.Year == representedDay.Year
                    && startTime.Month == representedDay.Month
                    && startTime.Day == representedDay.Day
                    && numberOfEventsShowing < 1)
                {
                    EventLine eventToShow = Instantiate(eventLinePrefab);
                    eventToShow.transform.SetParent(GetComponentInChildren<Canvas>().transform, false);
                    numberOfEventsShowing++;
                    eventToShow.eventTitleTextField.text = calendarEvent.summary;
                    DateTime endTime = DateTime.ParseExact(calendarEvent.end.dateTime, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
                    eventToShow.eventTimeTextField.text = startTime.ToString("HH:mm") + "\n" + endTime.ToString("HH:mm");
                }
            }
        }
        yield return new WaitForSeconds(1);
    }
}
