using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public CharacterProfile npcProfile;

    void Start()
    {
        npcProfile = new CharacterProfile
        {
            characterName = "Groundhog",
            personality = "Curious and forgetful",
            knowledgeBase = "Basic knowledge about the house and his past life",
            speechStyle = "Casual and slightly confused",
            attitude = "Friendly and helpful",
            desires = "To remember his past and get out of the house",
            viewOfLife = "Life is a mystery to be solved"
        };
    }
}
