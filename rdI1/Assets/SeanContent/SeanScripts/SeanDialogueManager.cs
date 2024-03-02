using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SeanDialogueManager : Singleton<SeanDialogueManager>
{
    #region Serialized Fields
    [SerializeField]
    DialogueSceneScriptableObject dialogueInformation;
    [SerializeField]
    GameObject dialogueBox;// UI panel containing dialogue text and character name/icon.

    [SerializeField]
    Image characterIcon; // UI element for displaying character's icon during dialogue.
    [SerializeField]
    TextMeshProUGUI characterName; // Displays the name of the speaking character.
    [SerializeField]
    TextMeshProUGUI dialogueArea; // Where the dialogue text is shown.
    [SerializeField]
    Action OnDialogueComplete; // Event triggered when dialogue is complete.
    #endregion


    private static Dictionary<string, bool> playedDialogues = new Dictionary<string, bool>();// Tracks dialogues that have already been played per scene.

    private bool completeCurrentSentence = false;



    private Queue<DialogueLine> lines;// Queue to hold all dialogue lines for the current dialogue session.

    public float typingSpeed = 0.01f;// Speed at which characters are shown in the dialogue box.

    private bool isTyping;// Flag to check if the text is currently being typed out.



    private void Awake()
    {
        lines = new Queue<DialogueLine>(); // Initialize the queue

    }
    private void Start()
    {
        StartDialogue(dialogueInformation.lines);
    }


    public void StartDialogue(List<DialogueLine> dialogueLines)
    {
        Time.timeScale = 0;// Pauses the game by setting time scale to 0.
        SeanAudioManager.Instance.Play(AudioNames.BackgroundMusic, true);

        lines.Clear();
        foreach (DialogueLine dialogueLine in dialogueLines)// Enqueue each line of the dialogue
        {
            lines.Enqueue(dialogueLine);
        }
        DisplayNextDialogueLine();// Starts displaying the dialogue lines.
    }


    public void OnButtonClick()
    {
        Debug.Log("Button was clicked");
        // If the dialogue box is clicked or a button is pressed, display the next line.
        DisplayNextDialogueLine();
    }


    void DisplayNextDialogueLine()
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DisplayNextDialogueLine();
        }
    }


    void EndDialogue()
    {
        SeanAudioManager.Instance.Stop(0);
        SceneManager.LoadScene(dialogueInformation.NextScene);


    }

}
