using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D other)//other是被攻击的那个人
    {
        //()?语法糖的判断，相当于不为空的判断
        other.GetComponent<Character>()?.TakeDamage(this);//this指针指向被调用的对象(当前类的实例)
    }
}
