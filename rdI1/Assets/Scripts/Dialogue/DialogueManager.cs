using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;// Singleton pattern to ensure only one instance exists.

    private static Dictionary<string, bool> playedDialogues = new Dictionary<string, bool>();// Tracks dialogues that have already been played per scene.

    private bool completeCurrentSentence = false;

    public GameObject dialogueBox;// UI panel containing dialogue text and character name/icon.

    public Image characterIcon; // UI element for displaying character's icon during dialogue.
    public TextMeshProUGUI characterName; // Displays the name of the speaking character.
    public TextMeshProUGUI dialogueArea; // Where the dialogue text is shown.
    public event Action OnDialogueComplete; // Event triggered when dialogue is complete.


    private Queue<DialogueLine> lines;// Queue to hold all dialogue lines for the current dialogue session.

    public bool isDialogueActive = false;// Flag to check if dialogue is currently active.

    public float typingSpeed = 0.01f;// Speed at which characters are shown in the dialogue box.

    private bool isTyping;// Flag to check if the text is currently being typed out.



    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
        }

        lines = new Queue<DialogueLine>(); // Initialize the queue.


    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;// Returns true if dialogue is active
    }

    public void StartDialogue(Dialogue dialogue)
    {
        // Checks if the dialogue for the current scene has already been played.
        string sceneName = SceneManager.GetActiveScene().name;


        if (!playedDialogues.ContainsKey(sceneName) || !playedDialogues[sceneName])
        {
            playedDialogues[sceneName] = true;

            isDialogueActive = true;
            dialogueBox.SetActive(true);
            Time.timeScale = 0;// Pauses the game by setting time scale to 0.

            AudioManager.Instance.Play(0, "bg", true);// Plays background dialogue audio.

            lines.Clear();
            foreach (DialogueLine dialogueLine in dialogue.dialogueLines)// Enqueue each line of the dialogue
            {
                lines.Enqueue(dialogueLine);
            }


            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.HideCountdownTimer();
            }

            DisplayNextDialogueLine();// Starts displaying the dialogue lines.
        }
    }


    public void OnButtonClick()
    {
        // If the dialogue box is clicked or a button is pressed, display the next line.
        if (isDialogueActive)
        {
            DisplayNextDialogueLine();
        }
    }


    public void DisplayNextDialogueLine()
    {
        // If currently typing, complete the current sentence immediately
        if (isTyping)
        {
            completeCurrentSentence = true;// Ensures the current typing coroutine is stopped in TypeSentence coroutine.
            return;
        }

        // If there are no more lines to display, end the dialogue.
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Dequeues the next line and updates UI elements accordingly.
        DialogueLine currentLine = lines.Dequeue();
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        // Starts typing out the next dialogue line.
        StopAllCoroutines();// Ensures no other typing coroutines are running.
        StartCoroutine(TypeSentence(currentLine));
    }


    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";// Clears the current text.
        isTyping = true;
        completeCurrentSentence = false;

        // Types out each character in the dialogue line at the specified typing speed
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            if (completeCurrentSentence)
            {
                dialogueArea.text = dialogueLine.line;
                break;
            }
            else
            {
                dialogueArea.text += letter;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
        }

        isTyping = false;// Indicates that typing is complete.
        completeCurrentSentence = false;
    }

    void Update()
    {
        // Checks for user input to display the next dialogue line.
        if (Input.GetKeyDown(KeyCode.Z) && isDialogueActive)
        {
            DisplayNextDialogueLine();
        }
    }


    void EndDialogue()
    {
        // Cleans up after dialogue is complete.
        isDialogueActive = false;
        dialogueBox.SetActive(false);
        Time.timeScale = 1; // Resume game

        // Switches audio back to boss fight music.
        AudioManager.Instance.Stop(0);
        AudioManager.Instance.Play(0, "bossFight", false);

        OnDialogueComplete?.Invoke();// Triggers the OnDialogueComplete event

        // Reactivates the anxiety meter if it exists in the scene
        AnxietyMeter anxietyMeter = FindObjectOfType<AnxietyMeter>();
        if (anxietyMeter != null)
        {
            anxietyMeter.ActivateMeter();
        }

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ShowCountdownTimer();
        }
    }
}

// Helper classes to define the structure of dialogue lines and characters.
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
