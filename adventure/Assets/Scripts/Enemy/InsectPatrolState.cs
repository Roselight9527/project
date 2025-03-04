using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        //∑¢œ÷player«–ªªchase
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.Switchstate(NPCstate.Chase);
        }
        if (!currentEnemy.physicsCheck.isGround || ((currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x <= 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x >= 0)))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
        //Debug.Log("Exit");
    }
}