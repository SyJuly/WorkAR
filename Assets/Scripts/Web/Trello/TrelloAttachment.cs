using System;

[Serializable]
public struct TrelloAttachmentResponse
{
    public TrelloAttachment trelloAttachment;
}

[Serializable]
public struct TrelloCardAttachmentsResponse
{
    public TrelloAttachment[] trelloAttachments;
}

[Serializable]
public class TrelloAttachment
{
    public string url;
    public TrelloAttachmentPreview[] previews;
}

[Serializable]
public class TrelloAttachmentPreview
{
    public string url;
}

