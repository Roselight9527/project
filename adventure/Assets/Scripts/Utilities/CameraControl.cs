using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("�¼�����")]
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
    //�����л������
    //private void Start()
    //{
    //    lastPos = transform.position;//�����ʼλ��
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
        //���������һ֡�͵�ǰ֮֡����ƶ�����
        //Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);
        //��������ƶ����룬�ƶ�����λ��
        //middleBackground.position += new Vector3(amountToMove.x * 0.5f, amountToMove.y * 0.5f, 0f);
        //lastPos = transform.position;
    }
}
