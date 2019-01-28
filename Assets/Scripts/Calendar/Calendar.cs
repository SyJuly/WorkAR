using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    private MonthCalendar monthCalendar;

    private void Start()
    {
        monthCalendar = GetComponentInChildren<MonthCalendar>();
        StartCoroutine(UpdateCalendar());
    }

    public IEnumerator UpdateCalendar()
    {
        WebManager.Instance.Google.Reader.SetEventsUpdater(UpdateEvents);
        while (true)
        {
            StartCoroutine(WebManager.Instance.Google.Reader.GetCalendarEvents());
            yield return new WaitForSeconds(10);
        }
    }

    private void UpdateEvents(GoogleCalendarEvent[] newEvents)
    {
        monthCalendar.UpdateEvents(newEvents);
    }
}
