using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;//单例模式
    public GameObject dialogueBox;
    public Text dialogueText, nameText;
    [TextArea(1, 3)]
    public string[] dialogueLines;
    [SerializeField] private int currentLine;
    [SerializeField] private float textSpeed;

    private bool isScrolling;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dialogueText.text = dialogueLines[currentLine];
    }
    private void Update()
    {
        if (dialogueBox.activeInHierarchy)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if(isScrolling==false)
                {
                    currentLine++;
                    if (currentLine < dialogueLines.Length)
                    {
                        CheckName();
                        StartCoroutine(ScrollingText()); 
                    }
                    else
                    {
                        dialogueBox.SetActive(false);
                        FindObjectOfType<PlayerController>().canMove = true;
                    }
                }
            }
        }
    }
    public void ShowDialogue(string[] _newLines, bool _hasname)
    {
        dialogueLines = _newLines;
        currentLine = 0;

        CheckName();

        //dialogueText.text = dialogueLines[currentLine]; //line by line
        StartCoroutine(ScrollingText());//协程函数
        dialogueBox.SetActive(true);
        nameText.gameObject.SetActive(_hasname);

        FindObjectOfType<PlayerController>().canMove = false;
    }
    public void ShowGuide(string[] _newLines, bool _hasname)
    {
        dialogueLines = _newLines;
        currentLine = 0;

        CheckName();

        //dialogueText.text = dialogueLines[currentLine]; //line by line
        StartCoroutine(ScrollingText());//协程函数
        dialogueBox.SetActive(true);
        nameText.gameObject.SetActive(_hasname);
    }
    public void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
    }
    public void CheckName()//判断对话对象
    {
        if (dialogueLines[currentLine].StartsWith("n-"))//删选出含有"n-"的对象
        {
            nameText.text = dialogueLines[currentLine].Replace("n-", "");//删除"n-"
            currentLine++;
        }
    }
    private IEnumerator ScrollingText()
    {
        isScrolling = true;
        dialogueText.text = "";

        foreach (char letter in dialogueLines[currentLine].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isScrolling = false;
    }
}
