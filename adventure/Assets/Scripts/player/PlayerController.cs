using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
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
    [Header("基本参数")]
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
    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    [Header("状态")]
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
        //获取Rigidbody2D组件
        rb = GetComponent<Rigidbody2D>();
        //获取其他代码中变量使用GetComponent方法（公开变量）
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        playerSizeVector = new Vector2(coll.offset.x, coll.offset.y);
        playerOffsetVector = new Vector2(coll.size.x, coll.size.y);
        inputControl = new PlayerInputControl();
        //跳跃
        inputControl.Gameplay.Jump.started += Jump;
        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
        //闪避
        inputControl.Gameplay.Dogde.started += PlayerDogde;
        //下蹲
        inputControl.Gameplay.Squat.started += PlayerSquat;
        //蓄力
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
        //在input action读取到设置好的move
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
    //测试
    /*private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
    }*/
    #region PlayerBehavior
    //场景加载过程中停止人物控制
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }
    //场景加载过程中启用人物控制
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
        //配置速度
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        //设置变量记录人物面朝方向
        int faceDir = (int)transform.localScale.x;//强制转换方法
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        //人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    //闪避
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
    //跳跃
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
                isChargeFinish = true; // 达到蓄力时间上限
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
    //攻击
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (!isDogding )
        {
            isAttack = true;
            playerAnimation.PlayAttack();
        }
    }
    //闪避冲刺
    private void PlayerDogde(InputAction.CallbackContext obj)
    {
        if (!isAttack && !isDodgeOnCooldown)
        {
            isDogding = true;
        }
    }
    //下蹲
    private void PlayerSquat(InputAction.CallbackContext obj)
    {
    }
    //蓄力
    private void PlayerCharge(InputAction.CallbackContext obj)
    {
    }

    //受击反弹的实现
    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 1).normalized;//归一化
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }
    //攻击反弹的实现
    public void Getattack(Transform attacker)
    {
        isAttack = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;//归一化
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
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;//三目运算符
    }
}