using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Talkable : MonoBehaviour
{
    [SerializeField] private bool isEnterned=false;
    [TextArea(1, 3)]
    public string[] lines;

    [SerializeField] private bool hasName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isEnterned = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEnterned = false;
        }
    }
    private void Update()
    {
        if((isEnterned) && (Keyboard.current.fKey.wasPressedThisFrame) && (DialogueManager.instance.dialogueBox.activeInHierarchy==false))
        {
            DialogueManager.instance.ShowDialogue(lines,hasName);
        }
    }
}
