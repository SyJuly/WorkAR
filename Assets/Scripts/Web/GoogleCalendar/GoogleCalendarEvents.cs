﻿using System;

[Serializable]
public class GoogleCalendarEventsResponse
{
    public string updated;
    public GoogleCalendarEvent[] items;
}

[Serializable]
public class GoogleCalendarEvent
{
    public GoogleCalendarEvent(string summary, DateTime start, DateTime end)
    {
        this.summary = summary;
        this.start = new GoogleTime(start);
        this.end = new GoogleTime(end);
    }
    //public string id;
    public string summary;
    public GoogleTime start;
    public GoogleTime end;
    //public string sequence;
}

[Serializable]
public class GoogleTime
{
    public GoogleTime(DateTime dateTimeToString)
    {
        dateTime = dateTimeToString.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }
    public string dateTime;
}