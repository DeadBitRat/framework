using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.Linq;

public class ChatBoxManager : MonoBehaviour
{
    public TMP_Text aiResponseText; // Text field for AI responses
    public TMP_InputField userInputField; // Input field for user input
    public Button submitButton; // Button for submitting input

    public AICharacterDesign aiCharacter; // AI Character Design ScriptableObject

    private string apiKey = " "; // Your ChatGPT API key
    private string apiUrl = "https://api.openai.com/v1/chat/completions"; // Correct API endpoint for ChatGPT

    public int maxWords = 50; // Maximum number of words per response
    public int maxSentences = 4; // Maximum number of sentences per response


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
        // Create the system message based on the AI character design
        string systemMessage = $"You are {aiCharacter.characterName}, inspired by {aiCharacter.inspiredBy}. " +
                               $"Act as naturally as possible, as if you are {aiCharacter.characterName}. " +
                               $"Stay in character and respond to the user as {aiCharacter.characterName} would. " +
                               $"You have the following traits: {aiCharacter.personality}. " +
                               $"Your knowledge includes: {aiCharacter.knowledge}. " +
                               $"Your history: {aiCharacter.history}. " +
                               $"You like: {aiCharacter.likes}, and dislike: {aiCharacter.dislikes}. " +
                               $"Your phobias are: {aiCharacter.phobias}. " +
                               $"You love: {aiCharacter.loves}, and desire: {aiCharacter.desires}. " +
                               $"You are a {aiCharacter.race} and your job is {aiCharacter.job}. " +
                               $"Don't ask to assist. Chat like a regular person following your personality" +
                               $"Try to keep your responses to no more than {maxSentences} sentences" +
                               $"Try to keep your responses to no more than {maxWords} words" +
                               $"Avoid acting like an AI assistant and do not discuss topics unrelated to the conversation at hand.";

        // Create the request body
        RequestBody requestBody = new RequestBody
        {
            model = "gpt-3.5-turbo",
            messages = new Message[]
            {
                new Message { role = "system", content = systemMessage },
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
            string aiText = response.choices[0].message.content.Trim();

            // Enforce max words and sentences limits
            aiText = EnforceLimits(aiText, aiCharacter.maxWords, aiCharacter.maxSentences);

            aiResponseText.text = aiText;
        }
    }

    private string EnforceLimits(string text, int maxWords, int maxSentences)
    {
        // Limit the number of words
        string[] words = text.Split(' ');
        if (words.Length > maxWords)
        {
            text = string.Join(" ", words.Take(maxWords));
        }

        // Limit the number of sentences
        string[] sentences = text.Split('.');
        if (sentences.Length > maxSentences)
        {
            text = string.Join(". ", sentences.Take(maxSentences)) + ".";
        }

        return text;
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
