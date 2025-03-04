using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Guidable : MonoBehaviour
{
    [SerializeField] private bool isEnterned = false;
    [TextArea(1, 3)]
    public string[] lines;

    [SerializeField] private bool hasName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            isEnterned = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEnterned = false;
            DialogueManager.instance.CloseDialogueBox();
        }
    }
    private void Update()
    {
        if ((isEnterned) && (DialogueManager.instance.dialogueBox.activeInHierarchy == false))
        {
            DialogueManager.instance.ShowGuide(lines, hasName);
        }
    }
}
