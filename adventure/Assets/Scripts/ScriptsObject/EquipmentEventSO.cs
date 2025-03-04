using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/EquipmentEventSO")]
public class EquipmentEventSO : ScriptableObject
{
    public UnityAction<ItemData, bool> OnEventRaised;

    public void RaiseEvent(ItemData equipment,bool isEquip)
    {
        OnEventRaised?.Invoke(equipment,isEquip);
    }
}
