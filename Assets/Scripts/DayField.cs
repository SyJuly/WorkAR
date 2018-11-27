using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DayField : MonoBehaviour {
    
    public DateTime representedDay;

    GoogleCalendar googleCalendar;

    [SerializeField]
    TextMeshPro dayNumberTextField;

    [SerializeField]
    TextMeshPro eventTextField;

    void Start () {
        googleCalendar = GetComponentInParent<GoogleCalendar>();
    }
	
	void Update () {
        dayNumberTextField.text = representedDay.Day.ToString();
        StartCoroutine(UpdateEvent());
    }

    IEnumerator UpdateEvent()
    {
        if (googleCalendar.events != null)
        {
            foreach(GoogleCalendarEvent calendarEvent in googleCalendar.events)
            {
                DateTime startTime = DateTime.ParseExact(calendarEvent.start.dateTime, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
                if (startTime.Day == representedDay.Day)
                {
                    eventTextField.text = calendarEvent.summary;
                }
            }
        }
        yield return new WaitForSeconds(1);
    }
}
