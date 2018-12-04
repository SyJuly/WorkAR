using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DayField : MonoBehaviour, IFocusable
{
    
    public DateTime representedDay;

    ReadFromGoogleCalendar googleCalendarReader;

    [SerializeField]
    TextMeshProUGUI dayNumberTextField;

    [SerializeField]
    TextMeshProUGUI eventTitleTextField;

    EventCreationButton eventCreationButton;

    void Start () {
        googleCalendarReader = GetComponentInParent<ReadFromGoogleCalendar>();
        eventCreationButton = GetComponentInChildren<EventCreationButton>();
        eventCreationButton.gameObject.SetActive(false);
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
                    && startTime.Day == representedDay.Day)
                {
                    eventTitleTextField.text = calendarEvent.summary;
                }
            }
        }
        yield return new WaitForSeconds(1);
    }

    public void OnFocusEnter()
    {
        eventCreationButton.gameObject.SetActive(true);
    }

    public void OnFocusExit()
    {
        eventCreationButton.gameObject.SetActive(false);
    }
}
