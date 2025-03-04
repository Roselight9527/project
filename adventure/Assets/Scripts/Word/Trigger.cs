using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Trigger : MonoBehaviour
{
    [Header("广播")]
    public ViodEventSO onEvent;
    [Header("监听事件")]
    public ViodEventSO onEventtrigger;
    [TextArea(1, 3)]
    public string[] lines; // 对话内容
    [SerializeField] private bool hasName; // 是否有名字显示
    [SerializeField] private bool isTrigger; // 是否允许触发
    [SerializeField] private bool isGate;
    [SerializeField] private int wordid;
    private TextMeshPro textMeshPro; // 3D TextMeshPro
    private Iinteractable targetItem;
    [SerializeField] private bool hasTriggered = false; // 标记是否已经触发过对话

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        if (isGate)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }
    }
    private void OnEnable()
    {
        if (onEventtrigger != null) // 确保 onEventtrigger 不是空的
        {
            onEventtrigger.OnEventRaised += OnEventtrigger;
        }
    }
    private void OnDisable()
    {
        if (onEventtrigger != null) // 确保 onEventtrigger 不是空的
        {
            onEventtrigger.OnEventRaised -= OnEventtrigger;
        }
    }
    private void OnEventtrigger()
    {
        if(isGate)
        {
            StartCoroutine(MoveUpSmoothly(5f, 1f));
        }
    }

    private IEnumerator MoveUpSmoothly(float distance, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * distance;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        //transform.position = targetPosition; // 确保最终到达目标位置
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PushTrigger") && isTrigger && !hasTriggered) // 检查是否已经触发过
        {
            targetItem = other.GetComponent<Iinteractable>();
            if (targetItem != null)
            {
                targetItem.TriggerAction();
                textMeshPro.color = Color.white;
                onEvent?.OnEventRaised();
                // 显示对话并标记为已触发
                DialogueManager.instance.ShowGuide(lines, hasName);
                GameManager.Instance.GetWord(wordid);
                hasTriggered = true;
            }
            else
            {
                Debug.LogWarning("No Iinteractable component found on object: " + other.name);
            }
        }
    }
}
