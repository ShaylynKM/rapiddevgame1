using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private static bool dialoguePlayed = false;

    public static DialogueManager Instance;

    private static Dictionary<string, bool> playedDialogues = new Dictionary<string, bool>();

    
    public GameObject dialogueBox;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;
    
    public bool isDialogueActive = false;

    public float typingSpeed = 0.01f;

    private bool isTyping;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        lines = new Queue<DialogueLine>();

    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        
        if (!playedDialogues.ContainsKey(sceneName) || !playedDialogues[sceneName])
        {
            playedDialogues[sceneName] = true; 

            isDialogueActive = true;
            dialogueBox.SetActive(true);
            Time.timeScale = 0;
            AudioManager.Instance.Play(0, "bg", true);

            lines.Clear();
            foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
            {
                lines.Enqueue(dialogueLine);
            }

            DisplayNextDialogueLine();
        }
    }


    public void OnButtonClick()
    {
        if (isDialogueActive)
        {
            DisplayNextDialogueLine();
        }
    }


    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (isTyping)
        {
            CompleteSentence();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        isTyping = true;

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }


    private void CompleteSentence()
    {
        StopAllCoroutines();

        if (lines.Count > 0)
        {
            DialogueLine currentLine = lines.Peek();
            dialogueArea.text = currentLine.line;
            isTyping = false;

            // remove the current row and prepare to display the next row
            lines.Dequeue();

            if (lines.Count == 0)
            {
                EndDialogue();
            }
        }
    }


    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Z) && isDialogueActive)
        {
            DisplayNextDialogueLine();
        }
    }


    void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);
        Time.timeScale = 1; // Resume game

        AudioManager.Instance.Stop(0);
        AudioManager.Instance.Play(0, "bossFight", false);


    }
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
