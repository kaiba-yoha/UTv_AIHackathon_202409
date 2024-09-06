using System;

[Serializable]
public class GPTChatMessage
{
    public string role;
    public string content;
}

[Serializable]
public class GPTChatBody
{
    public string model;
    public GPTChatMessage[] messages;
    public int max_tokens;
    public float temperature;
    public float top_p;
    public float frequency_penalty;
    public float presence_penalty;
}

[Serializable]
public class GPTResponse
{
    public string id;
    public string obj;
    public int created;
    public string model;
    public ChatUsage usage;
    public ChatChoice[] choices;
}

[Serializable]
public class ChatUsage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}

[Serializable]
public class ChatChoice
{
    public GPTChatMessage message;
    public int index;
    public string finish_reason;
}
