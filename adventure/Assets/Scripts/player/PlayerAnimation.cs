using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        SetAnimation();
    }
    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }
    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }
    public void SetAnimation()
    {
        //绑定animator中的变量
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));//取绝对值方法，防止出现负数
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isPush", physicsCheck.isPush);
        anim.SetBool("isDead", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
        anim.SetBool("isDogde", playerController.isDogding);
        anim.SetBool("isSquat", playerController.isSquat);
        anim.SetBool("isCharge", playerController.isCharge);
        anim.SetBool("isChargeFinish", playerController.isChargeFinish);
    }
}
