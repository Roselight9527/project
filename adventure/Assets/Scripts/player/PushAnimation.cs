
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushAnimation : StateMachineBehaviour
{
    private PlayerController playerController; // 缓存 PlayerController 引用
    private Animator animator; // 缓存 Animator 引用

    // OnStateEnter is called when进入动画状态时的处理
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        playerController = animator.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.speed = 100; // 初始化速度
        }
        animator.speed = 1f; // 确保动画正常播放
    }

    // OnStateUpdate 每帧更新动画状态
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerController == null) return;

        // 检测按键状态
        if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
        {
                animator.speed = 1f; // 恢复动画播放速度
        }
        else
        {
            animator.speed = 0f; // 暂停动画
        }
    }

    // OnStateExit 离开状态时的处理
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerController != null)
        {
            playerController.speed = 280; // 恢复速度
        }
        animator.speed = 1f; // 离开状态时恢复动画速度
    }
}