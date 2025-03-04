using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private bool isTrigger; // �Ƿ�������
    [SerializeField] private int wordid;
    private bool hasTriggered = false; // ����Ƿ��Ѿ��������Ի�
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&& isTrigger && !hasTriggered)
        {
            GameManager.Instance.GetWord(wordid);
            hasTriggered = true;
        }
    }
}
