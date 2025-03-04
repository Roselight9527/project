using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("����")]
    public ViodEventSO newGameEvent;
    public EquipmentEventSO equipmentEvent;
    [Header("��������")]
    public bool isPlayer;
    public float maxHealth;
    public float currentHealth;
    [Header("�����޵�")]//ͨ����ʱ����������޵е�Ŀ��
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnAttack;
    public UnityEvent<Transform> OnTakeDamage;//unity�Դ�ִ���¼��ķ���
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
            invulnerableCounter -= Time.deltaTime;//deltaTime:�����һ֡����ʱ��
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
            return;//����ܵ�һ���˺���ִ���·�����
        //Debug.Log(attacker.damage);
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //ִ������
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            //��������
            OnDie?.Invoke();
        }
        //���������޵е��ж�
        OnHealthChange?.Invoke(this);
        bool attackHit = DetermineAttackHit(attacker);
        if (attackHit)
        {
            // ����������У����� OnAttack �¼��������ݹ����߶���
            OnAttack?.Invoke(attacker.transform);
        }

    }
    private bool DetermineAttackHit(Attack attacker)//�жϹ����Ƿ�����
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

            OnHealthChange?.Invoke(this); //֪ͨui����
        }
    }
}
