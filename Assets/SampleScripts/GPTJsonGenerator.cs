using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GPTJsonGenerator : MonoBehaviour
{
    [SerializeField] private string apiKey; // OpenAIのAPIキー
    [SerializeField] private string apiUrl = "https://api.openai.com/v1/chat/completions";
    [SerializeField] private string model = "gpt-4o-2024-08-06"; // Structured Outputsをサポートするモデル

    public void SendMessageToAPI<T>(string userMessage,T structure, Action<T> callback)
    {
        StartCoroutine(SendRequest(userMessage, structure, callback));
    }

    private IEnumerator SendRequest<T>(string userMessage,T structure, Action<T> callback)
    {
        Message userMsg = new Message { role = "user", content = userMessage };
        List<Message> messages = new List<Message> { userMsg };
        RequestBody chatRequest = new RequestBody { model = model, messages = messages, response_format =JsonUtility.ToJson(structure) };

        string jsonData = JsonUtility.ToJson(chatRequest);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string responseJson = request.downloadHandler.text;
                T structuredResponse = JsonUtility.FromJson<T>(responseJson);
                callback(structuredResponse);
            }
        }
    }
}