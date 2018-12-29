using System;

[Serializable]
public struct TrelloAttachmentResponse
{
    public TrelloAttachment trelloAttachment;
}

[Serializable]
public class TrelloAttachment
{
    public TrelloAttachmentPreview[] previews;
}

[Serializable]
public class TrelloAttachmentPreview
{
    public string url;
}

