using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D other)//other�Ǳ��������Ǹ���
    {
        //()?�﷨�ǵ��жϣ��൱�ڲ�Ϊ�յ��ж�
        other.GetComponent<Character>()?.TakeDamage(this);//thisָ��ָ�򱻵��õĶ���(��ǰ���ʵ��)
    }
}
