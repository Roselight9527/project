using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour, Iinteractable
{
    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;
    public Sprite darkSprite;
    public Sprite lightSprite;

    public void TriggerAction()
    {
        spriteRenderer.sprite = lightSprite;
    }
    public void ExitAction()
    {
        spriteRenderer.sprite = darkSprite;
    }
}
