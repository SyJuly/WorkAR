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
    public string start;
    public string end;
    public string sequence;
}