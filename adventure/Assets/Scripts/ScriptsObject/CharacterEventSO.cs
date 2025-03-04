using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName ="Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;//事件订阅
    public void RaiseEvent(Character character)//事件调用
    {
        OnEventRaised?.Invoke(character);
    }
}
