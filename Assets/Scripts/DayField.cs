using System;
using System.Collections;
using System.Collections.Generic;
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
                DateTime startTime = DateTime.Parse(calendarEvent.start);
                Debug.Log("Event: " + startTime);
                Debug.Log("DateTime: " + representedDay.ToString());
                if (startTime.Day == representedDay.Day)
                {
                    eventTextField.text = calendarEvent.summary;
                }
            }
        }
        yield return new WaitForSeconds(1);
    }
}
