using UnityEngine;

[CreateAssetMenu(fileName = "NewAICharacter", menuName = "AI/AI Character Design")]
public class AICharacterDesign : ScriptableObject
{
    public string characterName;
    public string inspiredBy; // New field for inspiration
    public string personality;
    public string knowledge;
    public string history;
    public string likes;
    public string dislikes;
    public string phobias;
    public string loves;
    public string desires;
    public string race;
    public string job;
    public int maxWords = 50; // Maximum number of words per response
    public int maxSentences = 4; // Maximum number of sentences per response

    // Add more attributes as needed
}
