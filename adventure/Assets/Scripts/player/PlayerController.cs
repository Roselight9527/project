using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�����¼�")]
    public SceneLoadEventSO sceneLoadEvent;
    public ViodEventSO afterSceneLoadedEvent;
    public ViodEventSO loadDataEvent;
    public ViodEventSO backToMenuEvent;

    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private Vector2 inputDirection;
    private PlayerAnimation playerAnimation;
    public CapsuleCollider2D coll;
    public Vector2 playerSizeVector;
    public Vector2 playerOffsetVector;
    [Header("��������")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    public float attackForce;
    public float dodgeForce;
    public float dodgeTimer;
    public float dodgeDuration;
    public float dodgeCooldown = 2f;
    public float chargeTimer;
    public float chargeDuration;
    [Header("�������")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    [Header("״̬")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool isDogde;
    public bool isSquat;
    public bool isDogding;
    public bool isDodgeOnCooldown=false;
    public bool isCharge= false;
    public bool isChargeFinish=false;
    public bool canMove = true;
    private void Awake()
    {
        //��ȡRigidbody2D���
        rb = GetComponent<Rigidbody2D>();
        //��ȡ���������б���ʹ��GetComponent����������������
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        playerSizeVector = new Vector2(coll.offset.x, coll.offset.y);
        playerOffsetVector = new Vector2(coll.size.x, coll.size.y);
        inputControl = new PlayerInputControl();
        //��Ծ
        inputControl.Gameplay.Jump.started += Jump;
        //����
        inputControl.Gameplay.Attack.started += PlayerAttack;
        //����
        inputControl.Gameplay.Dogde.started += PlayerDogde;
        //�¶�
        inputControl.Gameplay.Squat.started += PlayerSquat;
        //����
        inputControl.Gameplay.Charge.started += PlayerCharge;
        inputControl.Enable();
    }
    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnafterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnafterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }

   

    private void Update()
    {
        //��input action��ȡ�����úõ�move
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
        Squat();
        if (!isDogde)
            Charge();
        //if (isAttack)
        //inputDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if(!isHurt && canMove)
        {
            Move();
        }

        if (!isAttack)
        {
            Dodge();
        }
    }
    //����
    /*private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
    }*/
    #region PlayerBehavior
    //�������ع�����ֹͣ�������
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }
    //�������ع����������������
    private void OnafterSceneLoadedEvent()
    {
        inputControl.Gameplay.Enable();
    }
    private void OnLoadDataEvent()
    {
        isDead = false;
    }
    public void Move()
    {
        //�����ٶ�
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        //���ñ�����¼�����泯����
        int faceDir = (int)transform.localScale.x;//ǿ��ת������
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        //���﷭ת
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    //����
    public void Dodge()
    {
        if(isDodgeOnCooldown)
        {
            dodgeTimer += Time.fixedDeltaTime;
            if(dodgeTimer >= dodgeCooldown)
            {
                isDodgeOnCooldown = false;
                dodgeTimer = 0f;
            }
        }
        if(isDogding)
        {
            if(!isDodgeOnCooldown)
            {
                if (dodgeTimer <= dodgeDuration)
                {
                    int faceDir = (int)transform.localScale.x;
                    Vector2 dodgeDirection = new Vector2(faceDir, 0);
                    rb.AddForce(dodgeDirection * dodgeForce, ForceMode2D.Impulse);
                    dodgeTimer += Time.fixedDeltaTime;
                }
                else
                {
                    isDogding = false;
                    isDodgeOnCooldown = true;
                    dodgeTimer = 0f;
                }
            }
        }
    }
    public void Squat()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            isSquat = !isSquat;
        }
        if (isSquat)
        {
            coll.size = new Vector2(1.35f,1f);
            coll.offset = new Vector2(0.01f,0.5f);
            coll.direction = CapsuleDirection2D.Horizontal;
        }
        else
        {
            coll.size = new Vector2(0.66f,1.57f);
            coll.offset = new Vector2(-0.01f,0.8f);
            coll.direction = CapsuleDirection2D.Vertical;
        }
    }
    //��Ծ
    public void Charge()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            isCharge = true;
            chargeTimer = 0f;
        }
        else if (Keyboard.current.kKey.wasReleasedThisFrame)
        {
            isCharge = false;
            chargeTimer = 0f;
            isChargeFinish = false;
        }

        if (isCharge && !isChargeFinish)
        {
            chargeTimer += Time.fixedDeltaTime;
            if (chargeTimer >= chargeDuration)
            {
                isChargeFinish = true; // �ﵽ����ʱ������
                chargeTimer = 0f;
            }
        }
        else if(isCharge && isAttack)
        {
                isChargeFinish = false;
                chargeTimer = 0f;
        }
    }
    #endregion
    private void Jump(InputAction.CallbackContext obj)
    {
        //Debug.Log("Jump");
        if (physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    //����
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (!isDogding )
        {
            isAttack = true;
            playerAnimation.PlayAttack();
        }
    }
    //���ܳ��
    private void PlayerDogde(InputAction.CallbackContext obj)
    {
        if (!isAttack && !isDodgeOnCooldown)
        {
            isDogding = true;
        }
    }
    //�¶�
    private void PlayerSquat(InputAction.CallbackContext obj)
    {
    }
    //����
    private void PlayerCharge(InputAction.CallbackContext obj)
    {
    }

    //�ܻ�������ʵ��
    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 1).normalized;//��һ��
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }
    //����������ʵ��
    public void Getattack(Transform attacker)
    {
        isAttack = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;//��һ��
        rb.AddForce(dir * attackForce, ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    #endregion
    public void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;//��Ŀ�����
    }
}