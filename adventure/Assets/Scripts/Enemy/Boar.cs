using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy//父类Enemy创建变量由子类Boar继承
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
    }
}
