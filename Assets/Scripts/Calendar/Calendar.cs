using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    public GoogleCalendarEvent[] events;

    private void Start()
    {
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
        events = newEvents;
    }
}
