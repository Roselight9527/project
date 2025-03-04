using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Trigger : MonoBehaviour
{
    [Header("�㲥")]
    public ViodEventSO onEvent;
    [Header("�����¼�")]
    public ViodEventSO onEventtrigger;
    [TextArea(1, 3)]
    public string[] lines; // �Ի�����
    [SerializeField] private bool hasName; // �Ƿ���������ʾ
    [SerializeField] private bool isTrigger; // �Ƿ�������
    [SerializeField] private bool isGate;
    [SerializeField] private int wordid;
    private TextMeshPro textMeshPro; // 3D TextMeshPro
    private Iinteractable targetItem;
    [SerializeField] private bool hasTriggered = false; // ����Ƿ��Ѿ��������Ի�

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
        if (onEventtrigger != null) // ȷ�� onEventtrigger ���ǿյ�
        {
            onEventtrigger.OnEventRaised += OnEventtrigger;
        }
    }
    private void OnDisable()
    {
        if (onEventtrigger != null) // ȷ�� onEventtrigger ���ǿյ�
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
            yield return null; // �ȴ���һ֡
        }

        //transform.position = targetPosition; // ȷ�����յ���Ŀ��λ��
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PushTrigger") && isTrigger && !hasTriggered) // ����Ƿ��Ѿ�������
        {
            targetItem = other.GetComponent<Iinteractable>();
            if (targetItem != null)
            {
                targetItem.TriggerAction();
                textMeshPro.color = Color.white;
                onEvent?.OnEventRaised();
                // ��ʾ�Ի������Ϊ�Ѵ���
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
