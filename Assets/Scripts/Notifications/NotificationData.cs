using System;

public class NotificationData
{
    public readonly string Header;
    public readonly string Text;
    
    public Action OnConfirm;
    public Action OnCancel;

    public NotificationData(string header, string text)
    {
        Header = header;
        Text = text;
    }
}

