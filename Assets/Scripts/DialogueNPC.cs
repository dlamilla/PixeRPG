using System.Collections;
using UnityEngine;
using TMPro;

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
    [SerializeField, TextArea(4,6)] private string[] dialogueLines;

    
    private AudioSource audioSource;
    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;  //Linea que mostramos

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = npcVoice;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            dialogueMark.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    private void SelectAudioClip()
    {
        if (isADialogue) { 
            if (lineIndex != 0)
            {
                isPlayerTalking = !isPlayerTalking;
            }

            audioSource.clip = isPlayerTalking ? playerVoice : npcVoice;
        }
    }

    private IEnumerator ShowLine()
    {
        SelectAudioClip();
        dialogueText.text = string.Empty;
        int charIndex = 0;

        foreach (char ch in dialogueLines[lineIndex])
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
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
