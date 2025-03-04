using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("�����¼�")]
    public ViodEventSO onEvent;

    public float moveDistance = 5f; // ����ƽ̨������ƶ��߶ȣ�0��5֮�䣩
    public float moveSpeed = 2f; // ����ƽ̨���ƶ��ٶ�
    private bool isMoving = false; // ����ƽ̨�Ƿ����ƶ�

    private Vector3 initialPosition; // ƽ̨�ĳ�ʼλ��
    private void OnEnable()
    {
        onEvent.OnEventRaised += OnEvent;
    }
    private void OnDisable()
    {
        onEvent.OnEventRaised -= OnEvent;
    }
    private void OnEvent()
    {
        isMoving = true;
    }
    void Start()
    {
        initialPosition = transform.position; // ��¼��ʼλ��
    }

    void Update()
    {
        if (isMoving)
        {
            // ��ƽ̨��Y�᷽���������ƶ�����Χ��0��5֮��
            float newYPosition = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            transform.position = new Vector3(initialPosition.x, initialPosition.y + newYPosition, initialPosition.z);
        }
    }
}
