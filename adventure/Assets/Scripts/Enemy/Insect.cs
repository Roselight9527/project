using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new InsectPatrolState();
        chaseState = new InsectChaseState();
    }
}
