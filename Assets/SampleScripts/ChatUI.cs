using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField]
    ChatGPTAPI textGenerator;

    [SerializeField]
    GPTJsonGenerator jsonGenerator;

    [SerializeField]
    InputField InputField;

    [SerializeField]
    Text text;

    [SerializeField,Multiline]
    string systemPrompt;

    List<GPTChatMessage> messages=new List<GPTChatMessage>();

    private void Awake()
    {
        messages.Add(new GPTChatMessage { content = systemPrompt, role = "system" });
    }

    public async void Send()
    {
        messages.Add(new GPTChatMessage { content = InputField.text, role = "user" });
        var res = await textGenerator.GetGPTResponse(messages.ToArray());
        messages.Add(new GPTChatMessage { content = res, role = "assistant" });
        

        text.text = res;
    }

    class JsonResponse
    {
        public string name;
        public string content;
    }

    public async void GetJson()
    {
        var str = new JsonResponse { name = "gpt", content = "hello world" };
        var res= await jsonGenerator.GetGPTResponse(
            new GPTChatMessage[] {
            new GPTChatMessage {content=systemPrompt, role="system"},
            new GPTChatMessage { content = InputField.text, role = "user" } }, str,new string[] {"name","content"});
        Debug.Log(res);
    }
}
