using UnityEngine;
using System;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue introDialogue;
    public Dialogue outroDialogue;
    public event Action OnOutroDialogueComplete; 

    public bool outroDialogueTriggered = false;

    public void TriggerIntroDialogue()
    {
        DialogueManager.Instance.StartDialogue(introDialogue);
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
