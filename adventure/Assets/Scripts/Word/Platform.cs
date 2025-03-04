using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("监听事件")]
    public ViodEventSO onEvent;

    public float moveDistance = 5f; // 控制平台的最大移动高度（0到5之间）
    public float moveSpeed = 2f; // 控制平台的移动速度
    private bool isMoving = false; // 控制平台是否在移动

    private Vector3 initialPosition; // 平台的初始位置
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
        initialPosition = transform.position; // 记录初始位置
    }

    void Update()
    {
        if (isMoving)
        {
            // 让平台在Y轴方向上上下移动，范围在0到5之间
            float newYPosition = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            transform.position = new Vector3(initialPosition.x, initialPosition.y + newYPosition, initialPosition.z);
        }
    }
}
