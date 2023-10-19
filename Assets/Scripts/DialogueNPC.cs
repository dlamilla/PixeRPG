using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueNPC : MonoBehaviour
{
    [Header("Audio NPC and Player")]
    [SerializeField] private AudioClip npcVoice;
    [SerializeField] private AudioClip playerVoice;
    [SerializeField] private bool isADialogue = true;
    [SerializeField] private float typingtime;
    [SerializeField] private int charsToPlaySound;
    private bool isPlayerTalking;

    [Header("Dialogues Máx 57 characters")]
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text interact;
    [SerializeField, TextArea(4,6)] private string[] dialogueLinesKeyboard;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLinesGamepad;


    private AudioSource audioSource;
    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;  //Linea que mostramos
    PlayerInput typeController;

    private void Start()
    {
        typeController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = npcVoice;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTalking(string typeControllerCurrent)
    {
        if (typeControllerCurrent == "Keyboard")
        {
            if (isPlayerInRange)
            {
                if (!didDialogueStart)
                {
                    StartDialogueKeyboard();
                }
                else if (dialogueText.text == dialogueLinesKeyboard[lineIndex])
                {
                    NextDialogueLineKeyboard();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueLinesKeyboard[lineIndex];
                }
            }
        }

        if (typeControllerCurrent == "Gamepad")
        {
            if (isPlayerInRange)
            {
                if (!didDialogueStart)
                {
                    StartDialogueGamepad();
                }
                else if (dialogueText.text == dialogueLinesGamepad[lineIndex])
                {
                    NextDialogueLineGamepad();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueLinesGamepad[lineIndex];
                }
            }
        }
    }

    //Keyboard
    private void StartDialogueKeyboard()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLineKeyboard());
    }

    private void NextDialogueLineKeyboard()
    {
        lineIndex++;
        if (lineIndex < dialogueLinesKeyboard.Length)
        {
            StartCoroutine(ShowLineKeyboard());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            dialogueMark.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    private IEnumerator ShowLineKeyboard()
    {
        SelectAudioClip();
        dialogueText.text = string.Empty;
        int charIndex = 0;

        foreach (char ch in dialogueLinesKeyboard[lineIndex])
        {
            dialogueText.text += ch;

            if(charIndex % charsToPlaySound == 0)
            {
                audioSource.Play();
            }

            charIndex++;
            yield return new WaitForSecondsRealtime(typingtime);
        }
    }

    //Gamepad
    private void StartDialogueGamepad()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLineGamepad());
    }

    private void NextDialogueLineGamepad()
    {
        lineIndex++;
        if (lineIndex < dialogueLinesGamepad.Length)
        {
            StartCoroutine(ShowLineGamepad());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            dialogueMark.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    private IEnumerator ShowLineGamepad()
    {
        SelectAudioClip();
        dialogueText.text = string.Empty;
        int charIndex = 0;

        foreach (char ch in dialogueLinesGamepad[lineIndex])
        {
            dialogueText.text += ch;

            if (charIndex % charsToPlaySound == 0)
            {
                audioSource.Play();
            }

            charIndex++;
            yield return new WaitForSecondsRealtime(typingtime);
        }
    }

    private void SelectAudioClip()
    {
        if (isADialogue)
        {
            if (lineIndex != 0)
            {
                isPlayerTalking = !isPlayerTalking;
            }

            audioSource.clip = isPlayerTalking ? playerVoice : npcVoice;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            string controllerInput = typeController.currentControlScheme;
            if (controllerInput == "Keyboard")
            {
                isPlayerInRange = true;
                dialogueMark.SetActive(true);
                interact.text = "[F]";
            }
            if (controllerInput == "Gamepad")
            {
                isPlayerInRange = true;
                dialogueMark.SetActive(true);
                interact.text = "[]";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
        }
    }
}
