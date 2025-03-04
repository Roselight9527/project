using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private bool isTrigger; // 是否允许触发
    [SerializeField] private int wordid;
    private bool hasTriggered = false; // 标记是否已经触发过对话
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&& isTrigger && !hasTriggered)
        {
            GameManager.Instance.GetWord(wordid);
            hasTriggered = true;
        }
    }
}
