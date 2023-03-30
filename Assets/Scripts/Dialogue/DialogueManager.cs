using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue Components")]
    public GameObject dialoguePanel;
    public Text speakerText;
    public Text dialogueText;

    Queue<DialogueLine> dialogueQueue;
    Action onDialogueEnd = null;

    bool isTyping = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue)
    {
        dialogueQueue = new Queue<DialogueLine>(dialogueLinesToQueue);

        UpdateDialogue();
    }
    
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue, Action onDialogueEnd)
    {
        StartDialogue(dialogueLinesToQueue);
        this.onDialogueEnd = onDialogueEnd; 
    }

    public void UpdateDialogue() 
    {
        if(isTyping)
        {
            isTyping = false;
            return;
        }

        dialogueText.text = string.Empty;

        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueQueue.Dequeue();

        Talk(line.speaker, line.message);
    }
    public void EndDialogue() 
    {
        dialoguePanel.SetActive(false);

        onDialogueEnd?.Invoke();

        onDialogueEnd = null;
    }

    public void Talk(string speaker, string message)
    {
        dialoguePanel.SetActive(true);

        speakerText.text = speaker;

        speakerText.transform.parent.gameObject.SetActive(speaker != "");

        //dialogueText.text = message;
        StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        char[] charsToType = textToType.ToCharArray();
        for(int i = 0; i < charsToType.Length; i++)
        {
            dialogueText.text += charsToType[i];
            yield return new WaitForEndOfFrame();

            if(!isTyping)
            {
                dialogueText.text = textToType;
                break;
            }
        }

        isTyping = false;
    }
    
    public static List<DialogueLine> CreateSimpleMessage(string message)
    {
        DialogueLine messageDialogueLine = new DialogueLine("", message);
        
        List<DialogueLine> listToReturn = new List<DialogueLine>();

        listToReturn.Add(messageDialogueLine);

        return listToReturn;
    }
}
