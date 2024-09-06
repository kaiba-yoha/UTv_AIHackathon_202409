using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class GPT4oTextGenerator : MonoBehaviour
{
    private const string API_URL = "https://api.openai.com/v1/chat/completions";
    private const string API_KEY = "あなたのAPIキーをここに入力";


    public void GenerateResponse(string userInput)
    {
        StartCoroutine(SendRequest(userInput));
    }

    private IEnumerator SendRequest(string userInput)
    {
        RequestBody requestBody = new RequestBody
        {
            messages = new List<Message>()
            {
                new Message { role = "user", content = userInput }
            }
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
                    Debug.Log("生成されたテキスト: " + generatedText);
                }
            }
        }
    }
}
