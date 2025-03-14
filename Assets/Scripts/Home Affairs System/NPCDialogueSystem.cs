using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueSystem : MonoBehaviour
{
    private Dictionary<string, string[]> npcDialogues = new Dictionary<string, string[]>
    {
        { "Officer", new string[] { "Come back tomorrow.", "You are missing some papers.", "Go to window 5 for more info." } },
        { "Refugee", new string[] { "Have you eaten today?", "I’ve been waiting here for months.", "The system is down again." } }
    };

    public void StartDialogue(string npcName)
    {
        if (npcDialogues.ContainsKey(npcName))
        {
            string dialogue = npcDialogues[npcName][UnityEngine.Random.Range(0, npcDialogues[npcName].Length)];
            Debug.Log(npcName + ": " + dialogue);
        }
        else
        {
            Debug.Log("NPC not found.");
        }
    }
}
