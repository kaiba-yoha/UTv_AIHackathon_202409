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

    [SerializeField]
    string systemPrompt;

    public async void Send()
    {
        var res = await textGenerator.GetGPTResponse(new GPTChatMessage[] {
            new GPTChatMessage {content=systemPrompt,role="system"},
            new GPTChatMessage { content = InputField.text, role = "user" } });

        text.text = res;
    }

    class JsonResponse
    {
        public string name;
        public string content;
    }

    public void GetJson()
    {
        var str = new JsonResponse { name = "gpt", content = "hello world" };
        jsonGenerator.SendMessageToAPI(InputField.text, str, (t) => { text.text = JsonUtility.ToJson(t); });
    }
}
