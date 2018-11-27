using System;

[Serializable]
public struct GoogleCalendarEventsResponse
{
    public string updated;
    public GoogleCalendarEvent[] items;
}

[Serializable]
public struct GoogleCalendarEvent
{
    public string id;
    public string summary;
    public GoogleTime start;
    public GoogleTime end;
    public string sequence;
}

[Serializable]
public struct GoogleTime
{
    public string dateTime;
}