using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Google
{
    private GoogleCalendarAPI api;
    public ReadFromGoogleCalendar Reader { get; private set; }
    public WriteToGoogleCalendar Writer { get; private set; }

    public Google()
    {
        api = new GoogleCalendarAPI();
        Reader = new ReadFromGoogleCalendar(api);
        Writer = new WriteToGoogleCalendar(api);
    }
}
