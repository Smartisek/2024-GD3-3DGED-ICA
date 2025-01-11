using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCConversation conversation;
    private bool conversationInRange = false;
    public bool inDialogue = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            conversationInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            conversationInRange = false;
            inDialogue = false;
        }
    }

    public void StartDialogue()
    {
        if (conversationInRange && !inDialogue)
        {
            inDialogue = true;
            ConversationManager.Instance.StartConversation(conversation);
            Debug.Log("Conversation started!");
        }
    }

    public void EndDialogue()
    {
        inDialogue = false;
    }

}
