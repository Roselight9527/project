using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("事件监听")]
    public ViodEventSO afterSceneLoadedEvent;

    public Transform target;
    public Transform farBackground, middleBackground;
    private Vector2 lastPos;
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public ViodEventSO cameraShakeEvent;
     
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
    //场景切换后更改
    //private void Start()
    //{
    //    lastPos = transform.position;//相机初始位置
    //    GetNewCameraBounds();
    //}
    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }


    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }
    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        confiner2D.InvalidateCache();
    }
    private void Update()
    {
        //计算相机上一帧和当前帧之间的移动距离
        //Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);
        //根据相机移动距离，移动背景位置
        //middleBackground.position += new Vector3(amountToMove.x * 0.5f, amountToMove.y * 0.5f, 0f);
        //lastPos = transform.position;
    }
}
