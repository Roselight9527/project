using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour , Iinteractable
{
    [Header("广播")]
    public ViodEventSO SaveDataEvent;
    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;

    public Sprite darkSprite;

    public Sprite lightSprite;

    public bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
    }

    public void TriggerAction()
    {
       if(!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            SaveDataEvent.RaiseEvent();

        }
    }

    public void ExitAction()
    {
    }
}
