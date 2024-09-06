
using System.Collections.Generic;

[System.Serializable]
public class ResponseData
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}


[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[System.Serializable]
public class RequestBody
{
    public string model = "gpt-4o";
    public List<Message> messages;
    public string response_format; // Structured Outputs‚ðŽw’è
}