using System;

[Serializable]
public class GoogleAccessToken
{
    public string access_token;
    public string expires_in;
    public string refresh_token;
    public string scope;
    public string token_type;
}