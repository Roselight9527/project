using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("监听")]
    public ViodEventSO newGameEvent;
    public EquipmentEventSO equipmentEvent;
    [Header("基本属性")]
    public bool isPlayer;
    public float maxHealth;
    public float currentHealth;
    [Header("受伤无敌")]//通过计时器达成受伤无敌的目的
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnAttack;
    public UnityEvent<Transform> OnTakeDamage;//unity自带执行事件的方法
    public UnityEvent OnDie;
    private void NewGame()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    }
    private void EquipMent(ItemData item, bool isequip)
    {if(isPlayer)
        {
            if (isequip)
            {
                maxHealth += item.health;
                currentHealth += item.health;
            }
            else
            {
                maxHealth -= item.health;
                currentHealth -= item.health;
            }
        }
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        equipmentEvent.OnEventRaised += EquipMent;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        equipmentEvent.OnEventRaised -= EquipMent;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;//deltaTime:完成上一帧所用时间
            if(invulnerableCounter<=0)
            {
                invulnerable = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Water"))
        {
            if (currentHealth > 0)
            {
                currentHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDie?.Invoke();
            }
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if(invulnerable)
            return;//如果受到一次伤害不执行下方代码
        //Debug.Log(attacker.damage);
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            //触发死亡
            OnDie?.Invoke();
        }
        //触发受伤无敌的判断
        OnHealthChange?.Invoke(this);
        bool attackHit = DetermineAttackHit(attacker);
        if (attackHit)
        {
            // 如果攻击命中，触发 OnAttack 事件，并传递攻击者对象
            OnAttack?.Invoke(attacker.transform);
        }

    }
    private bool DetermineAttackHit(Attack attacker)//判断攻击是否命中
    {
        float distance = Vector3.Distance(transform.position, attacker.transform.position);
        return distance < attacker.attackRange;
    }
private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    public DataDefinition GetdataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if(data.characterPosDict.ContainsKey(GetdataID().ID))
        {
            data.characterPosDict[GetdataID().ID] = transform.position;
            data.floatSaveData[GetdataID().ID + "health"] = this.currentHealth;
        }
        else
        {
            data.characterPosDict.Add(GetdataID().ID, transform.position);
            data.floatSaveData.Add(GetdataID().ID + "health", this.currentHealth);
        }
    }

    public void LoadData(Data data)
    {
        if(data.characterPosDict.ContainsKey(GetdataID().ID))
        {
            transform.position = data.characterPosDict[GetdataID().ID];
            this.currentHealth = data.floatSaveData[GetdataID().ID + "health"];

            OnHealthChange?.Invoke(this); //通知ui更新
        }
    }
}
