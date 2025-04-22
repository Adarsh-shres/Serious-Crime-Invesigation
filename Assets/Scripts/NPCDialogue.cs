using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    private int currentLine = 0;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueUI.activeSelf)
            {
                dialogueUI.SetActive(true);
                dialogueText.text = dialogueLines[currentLine];
            }
            else
            {
                currentLine++;
                if (currentLine < dialogueLines.Length)
                {
                    dialogueText.text = dialogueLines[currentLine];
                }
                else
                {
                    dialogueUI.SetActive(false);
                    currentLine = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueUI.SetActive(false);
            currentLine = 0;
        }
    }
}
