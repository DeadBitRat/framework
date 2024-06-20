using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

public class ChatBoxManager : MonoBehaviour
{
    public TMP_Text aiResponseText; // Text field for AI responses
    public TMP_InputField userInputField; // Input field for user input
    public Button submitButton; // Button for submitting input

    private string apiKey = " Api key va aqui"; // Your ChatGPT API key
    private string apiUrl = "https://api.openai.com/v1/chat/completions"; // Correct API endpoint for ChatGPT

    private void Start()
    {
        // Add listener to the submit button
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        // Get the user input
        string userInput = userInputField.text;
        // Check if the input is not empty
        if (!string.IsNullOrEmpty(userInput))
        {
            // Start the coroutine to get the AI response
            StartCoroutine(GetAIResponse(userInput));
            // Clear the input field after submitting
            userInputField.text = "";
        }
    }

    private IEnumerator GetAIResponse(string userInput)
    {
        // Create the request body
        RequestBody requestBody = new RequestBody
        {
            model = "gpt-3.5-turbo",
            messages = new Message[]
            {
                new Message { role = "system", content = "You are a cat. And you don't speak any human language. And you really want some food." },
                new Message { role = "user", content = userInput }
            }
        };

        string jsonRequestBody = JsonUtility.ToJson(requestBody, true);

        // Set up the web request
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            // Display error if request fails
            aiResponseText.text = "Error: " + request.error + "\nResponse: " + request.downloadHandler.text;
        }
        else
        {
            // Parse and display the response
            string jsonResponse = request.downloadHandler.text;
            OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(jsonResponse);
            aiResponseText.text = response.choices[0].message.content.Trim();
        }
    }

    // Classes to structure the request and response
    [System.Serializable]
    private class RequestBody
    {
        public string model;
        public Message[] messages;
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class OpenAIResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }
}
