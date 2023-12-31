using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float typingSpeed;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject instruction1;
    [SerializeField] private GameObject instructionN;
    private bool trigger = true;

    [SerializeField] private GameObject character1;
    [SerializeField] private GameObject character2;

    public bool levelEndMessage;
    public bool levelEnd;

    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject randomMessage;
    
    private void Start()
    {
        instruction1.SetActive(true);
    }

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            StartCoroutine(TypingInstructions(instruction1));
        }

        if (levelEndMessage)
        {
            levelEndMessage = false;
            StartCoroutine(TypingInstructions(instructionN));
        }

        if (randomMessage.activeSelf)
        {
            levelEnd = true;
        }

        if (!instructionN.activeSelf && levelEnd)
        {
            dialogueBox.SetActive(false);
            portal.SetActive(true);
        }
    }

    private IEnumerator StartTyping(TMP_Text currentText)
    {
        
        int charIndex = 0;
        currentText.gameObject.SetActive(true);
        string fullText = currentText.text;
        currentText.text = "";

        while (charIndex < fullText.Length)
        {
            currentText.text += fullText[charIndex];
            charIndex++;

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(1.5f);
        currentText.gameObject.SetActive(false);
    }

    private IEnumerator TypingInstructions(GameObject instruction)
    {
        if (instruction == null)
        {
            yield break;
        }

        int instructionCount = instruction.transform.childCount;

        for (int i = 0; i < instructionCount; i++)
        {
            ToggleImages();
            GameObject temp = instruction.transform.GetChild(i).gameObject;

            if (temp == null)
            {
                yield break;
            }

            if (temp.name != "Instruction")
            {
                continue;
            }
            else
            {
                temp.SetActive(true);
                string currentText = temp.GetComponent<TMP_Text>().text;
                temp.GetComponent<TMP_Text>().text = "";

                for (int j = 0; j < currentText.Length; j++)
                {
 
                    temp.GetComponent<TMP_Text>().text += currentText[j];
                    yield return new WaitForSeconds(typingSpeed);
                }

                yield return new WaitForSeconds(1.5f);
                temp.SetActive(false);
            }
            yield return new WaitForSeconds(0.1f);
        }
        instruction.SetActive(false);
    }

    private void ToggleImages()
    {
        if (character1.activeSelf)
        {
            character1.SetActive(false);
            character2.SetActive(true);
        } else if (character2.activeSelf)
        {
            character2.SetActive(false);
            character1.SetActive(true);
        }
    }
    
}
