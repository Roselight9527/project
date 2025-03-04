using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   protected Rigidbody2D rb;//所有子类可访问，外部不可访问
    [HideInInspector]public Animator anim;
    [HideInInspector] public PhysicsCheck physicsCheck;
    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;
    public float hurtForce;
    public Transform attacker;
    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;
    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;
    //状态参数
    protected BaseState patrolState;
    protected BaseState chaseState;
    private BaseState currentState;
    public delegate void OnDamageTaken();
    public event OnDamageTaken onDamageTaken;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        //waitTimeCounter = waitTime;
    }
    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this); 
    }
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x,0,0);
        //撞到墙实现翻转
        currentState.LogicUpdate();
        TimeCounter();
    }
    private void FixedUpdate()
    {
        if (!isHurt && !isDead && !wait)
            Move();
        currentState.PhysicsUpdate();
    }
    private void OnDisable()
    {
        currentState.OnExit();
    }
    public virtual void Move()//虚函数
    { 
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    private void TimeCounter()
    {
        if(wait)
        {
            waitTimeCounter -= Time.deltaTime;
            rb.velocity = new Vector2(0, rb.velocity.y);
            if(waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, transform.localScale.y, transform.localScale.z);
            }
        }
        if(!FoundPlayer() && lostTimeCounter>0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        //else
        //{
        //    lostTimeCounter = lostTime;
        //}
    }
    public bool FoundPlayer()//发射一个方形的判断器检测player
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0,faceDir,checkDistance,attackLayer);
    }
    //用switch语句枚举状态
    public void Switchstate(NPCstate state)
    {
        var newstate = state switch
        {
            NPCstate.Patrol => patrolState,
            NPCstate.Chase => chaseState,
            _ => null
        };
        currentState.OnExit();  //上一个状态的退出
        currentState = newstate;  //切换状态
        currentState.OnEnter(this);  //执行新状态
    }
    #region 事件执行方法
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        //转身
        if(attackTrans.position.x-transform.position.x>0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //受伤被击退
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
        onDamageTaken();
    }
   private IEnumerator OnHurt(Vector2 dir)//迭代器，协程程序返回值
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);//在dir方向添加一个基本力
        yield return new WaitForSeconds(0.5f);//等待0.5秒后执行下方代码
        isHurt = false;
    }
    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        isDead = true;
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        if (transform.localScale.x >= 0)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -1, 0), 0.2f);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * 1, 0), 0.2f);
        }
    }
    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }
}
