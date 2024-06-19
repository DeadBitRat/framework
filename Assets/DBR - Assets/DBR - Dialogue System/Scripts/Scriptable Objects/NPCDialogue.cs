using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "DBR Dialogue System/NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public List<string> actorsInScene;

    public DialogueLine[] lines;
}
