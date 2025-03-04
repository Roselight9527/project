using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public AudioSource scoreAudio;  // �÷�ʱ���ŵ���Ч

    GameManager manager;  // ��Ϸ����������
    bool addedScore;  // ����Ƿ��Ѿ��÷�

    void Start()
    {
        // ������Ϸ������
        manager = GameObject.FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // �������Ƿ�ͨ��������ؿ��Ų��һ�û�е÷�
        if (!other.gameObject.transform.root.CompareTag("Player") || addedScore)
            return;  // �����ײ���岻����ң����Ѿ��÷֣���ִ��

        // ���ӷ�����������Ч
        addedScore = true;  // ����ѵ÷�
        manager.UpdateScore(1);  // ���·���
        scoreAudio.Play();  // ���ŵ÷���Ч
    }
}