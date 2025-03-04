using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    GameManager manager;

    void Start()
    {
        manager = GameObject.FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter(Collision other)
    {
        // ��⵽��ײʱ�������������ϰ��﷢����ײ��������Ϸ
        if (other.gameObject.transform.root.CompareTag("Player"))
            manager.GameOver();  // ������Ϸ�������� GameOver ����������Ϸ
    }
}
