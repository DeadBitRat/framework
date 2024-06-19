using UnityEngine;

[CreateAssetMenu(fileName = "NewSimpleNPCDialogue", menuName = "DBR Dialogue System/Simple NPC Dialogue")]
public class SimpleNPCDialogue : ScriptableObject
{
    public SimpleDialogueLine[] sentences;
}