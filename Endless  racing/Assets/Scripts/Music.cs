using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    // ���ֹ���ʵ��
    private static Music instance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;  
        }
        else
        {
            Destroy(gameObject); 
        }
        DontDestroyOnLoad(this.gameObject);
    }
}