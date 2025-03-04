using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixScaleDuringAnimation : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        // �����ʼ������ֵ
        originalScale = transform.localScale;
    }

    void LateUpdate()
    {
        // �� LateUpdate �и�������ֵ��ȷ���ڶ���֮��ִ��
        transform.localScale = originalScale;
    }
}
