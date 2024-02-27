using UnityEngine;
using System;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Dialogue outroDialogue;
    public event Action OnOutroDialogueComplete; 

    public bool outroDialogueTriggered = false;

    private void Start()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public void TriggerOutroDialogue()
    {
        if (!outroDialogueTriggered)
        {
            DialogueManager.Instance.StartDialogue(outroDialogue);
            outroDialogueTriggered = true;
        }
    }

    public void OutroDialogueComplete()
    {
        OnOutroDialogueComplete?.Invoke();
    }
}
