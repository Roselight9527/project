using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    private static EquipManager _instance;
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static EquipManager Instance
    {
        get
        {
            return _instance;
        }
    }
}
