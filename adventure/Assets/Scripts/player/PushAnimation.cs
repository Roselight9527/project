
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushAnimation : StateMachineBehaviour
{
    private PlayerController playerController; // ���� PlayerController ����
    private Animator animator; // ���� Animator ����

    // OnStateEnter is called when���붯��״̬ʱ�Ĵ���
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        playerController = animator.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.speed = 100; // ��ʼ���ٶ�
        }
        animator.speed = 1f; // ȷ��������������
    }

    // OnStateUpdate ÿ֡���¶���״̬
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerController == null) return;

        // ��ⰴ��״̬
        if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
        {
                animator.speed = 1f; // �ָ����������ٶ�
        }
        else
        {
            animator.speed = 0f; // ��ͣ����
        }
    }

    // OnStateExit �뿪״̬ʱ�Ĵ���
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerController != null)
        {
            playerController.speed = 280; // �ָ��ٶ�
        }
        animator.speed = 1f; // �뿪״̬ʱ�ָ������ٶ�
    }
}