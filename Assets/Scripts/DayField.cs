using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DayField : MonoBehaviour {
    
    public DateTime representedDay;

    ReadFromGoogleCalendar googleCalendarReader;

    [SerializeField]
    TextMeshPro dayNumberTextField;

    [SerializeField]
    TextMeshPro eventTextField;

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
                if (startTime.Day == representedDay.Day)
                {
                    eventTextField.text = calendarEvent.summary;
                }
            }
        }
        yield return new WaitForSeconds(1);
    }
}
