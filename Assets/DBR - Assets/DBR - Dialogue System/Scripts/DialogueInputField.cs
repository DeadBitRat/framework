using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class DialogueInputField : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button submitButton;
    public DialoguePlayerManager dialoguePlayerManager;
    public NPCManager npcManager;

    // Fake API Key for demonstration, replace with your actual OpenAI API key
    private string openAIKey = "your-fake-api-key-for-demo";

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    void OnSubmit()
    {
        string playerQuestion = inputField.text;
        StartCoroutine(GetAIResponse(playerQuestion));
        inputField.text = "";
    }

    IEnumerator GetAIResponse(string question)
    {
        string prompt = $"You are {npcManager.npcProfile.characterName}, a {npcManager.npcProfile.personality} character. " +
                        $"You know {npcManager.npcProfile.knowledgeBase}. " +
                        $"You speak in a {npcManager.npcProfile.speechStyle} manner. " +
                        $"Your attitude is {npcManager.npcProfile.attitude}. " +
                        $"Your desires are {npcManager.npcProfile.desires}. " +
                        $"Your view of life is '{npcManager.npcProfile.viewOfLife}'. " +
                        $"Player: {question}";

        string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";

        UnityWebRequest www = new UnityWebRequest(apiUrl, "POST");
        www.SetRequestHeader("Authorization", $"Bearer {openAIKey}");
        www.SetRequestHeader("Content-Type", "application/json");

        var jsonData = new
        {
            prompt = prompt,
            max_tokens = 150
        };

        string jsonString = JsonUtility.ToJson(jsonData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var responseText = www.downloadHandler.text;
            var aiResponse = ParseAIResponse(responseText);
            dialoguePlayerManager.TalkToAINPC(aiResponse);
        }
        else
        {
            Debug.LogError(www.error);
        }
    }

    string ParseAIResponse(string jsonResponse)
    {
        // Parse the JSON response to extract the text field containing the AI's reply
        var jsonObject = JsonUtility.FromJson<AIResponse>(jsonResponse);
        return jsonObject.choices[0].text.Trim();
    }
}

[System.Serializable]
public class AIResponse
{
    public Choice[] choices;

    [System.Serializable]
    public class Choice
    {
        public string text;
    }
}
