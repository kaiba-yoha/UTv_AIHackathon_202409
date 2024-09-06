using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTAPI : MonoBehaviour
{
    [SerializeField]
    string API_KEY;

    [Tooltip("The maximum number of tokens to generate. Requests can use up to 4000 tokens shared between prompt and completion. (One token is roughly 4 characters for normal English text)")]
    public int max_tokens = 2048;

    [Range(0.0f, 1.0f), Tooltip("Controls randomness: Lowering results in less random completions. As the temperature approaches zero, the model will become deterministic and repetitive.")]
    public float temperature = 0.2f;

    [Range(0.0f, 1.0f), Tooltip("Controls diversity via nucleus sampling: 0.5 means half of all likelihood-weighted options are considered.")]
    public float top_p = 0.8f;

    [Tooltip("Where the API will stop generating further tokens. The returned text will not contain the stop sequence.")]
    public string stop;

    [Range(0.0f, 2.0f), Tooltip("How much to penalize new tokens based on their existing frequency in the text so far. Decreases the model's likelihood to repeat the same line verbatim.")]
    public float frequency_penalty = 0;

    [Range(0.0f, 2.0f), Tooltip("How much to penalize new tokens based on whether they appear in the text so far. Increases the model's likelihood to talk about new topics.")]
    public float presences_penalty = 0;

    public static int maxChatCount = 5;

    public async UniTask<string> GetGPTResponse(GPTChatMessage[] message)
    {
        try
        {
            string url = "https://api.openai.com/v1/chat/completions";

            GPTChatBody chatBody = new GPTChatBody
            {
                model = "gpt-4o",//"gpt-4-turbo-preview",//"gpt-3.5-turbo",
                messages = message,
                max_tokens = max_tokens,
                temperature = temperature,
                top_p = top_p,
                frequency_penalty = frequency_penalty,
                presence_penalty = presences_penalty
            };
            string myJson = JsonUtility.ToJson(chatBody);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(myJson);

            // var filePath = Path.Combine(Application.dataPath, "GameData", "openai_apikey.txt");
            // var privateApiKey = await File.ReadAllTextAsync(filePath);
            var privateApiKey = API_KEY;

            using UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + privateApiKey);
            www.SetRequestHeader("Content-Type", "application/json");

            await www.SendWebRequest().ToUniTask();
            var response = www.downloadHandler.text;
            Debug.Log(response);
            //JSON形式のresponseをBodyクラスに変換
            var responseJson = JsonUtility.FromJson<GPTResponse>(response);
            //Bodyクラスの中のmessagesの中のcontentを取得
            var output = responseJson.choices[0].message.content;
            return output;
        }
        catch (Exception e)
        {
            Debug.LogError($"エラー:{e}");
            return null;
        }
    }
}
