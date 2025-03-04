using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour, Iinteractable
{
    [Header("��������")]
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
