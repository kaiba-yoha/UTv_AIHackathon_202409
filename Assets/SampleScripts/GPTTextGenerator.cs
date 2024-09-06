using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System;

public class GPT4oTextGenerator : MonoBehaviour
{
    private const string API_URL = "https://api.openai.com/v1/chat/completions";
    private const string API_KEY = "";


    public void GenerateResponse(string userInput, Action<string> callback)
    {
        StartCoroutine(SendRequest(userInput, callback));
    }

    private IEnumerator SendRequest(string userInput, Action<string> callback)
    {
        RequestBody requestBody = new RequestBody
        {
            model="gpt-4o",
            messages = new List<Message>()
            {
                new Message { role = "system", content = "ユーザーの入力を徹底的に否定してください。あらゆる観点から否定的な意見を述べ、さらに京都人風の皮肉っぽい言い方で返答してください。" },
                new Message { role = "user", content = userInput }
            }
            ,
            response_format = ""
        };

        string jsonRequestBody = JsonUtility.ToJson(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + API_KEY);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("エラー: " + request.error);
            }
            else
            {
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
                if (responseData.choices != null && responseData.choices.Length > 0)
                {
                    string generatedText = responseData.choices[0].message.content;
                    callback(generatedText);
                    Debug.Log("生成されたテキスト: " + generatedText);
                }
            }
        }
    }
}
