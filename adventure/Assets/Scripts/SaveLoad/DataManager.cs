using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(order:-100)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("事件监听")]
    public ViodEventSO saveDataEvent;
    public ViodEventSO LoadDataEvent;


    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        saveData = new Data();
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        LoadDataEvent.OnEventRaised += Load;
    }
    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        LoadDataEvent.OnEventRaised -= Load;
    }
    private void Update()
    {
        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    public void RegisterSaveData(ISaveable saveable)
    { 
        if(!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }
    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }
    public void Save()
    {
        foreach(var saveable in saveableList)//告诉每一个接口执行保存数据操作
        {
            saveable.GetSaveData(saveData);
        }

        foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + "    " + item.Value);
        }
    }
    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }
}
