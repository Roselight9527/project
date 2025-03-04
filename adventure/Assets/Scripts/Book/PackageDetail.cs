using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PackageDetail : MonoBehaviour
{
    private Transform UIword;
    private Transform UImean;
    private Text Word;
    private Text Mean;

    private PackageLocalItem packageLocalData;
    private PackagePanel uiParent;
    [SerializeField] private int itemID;
    [SerializeField] private string itemUid;
    [SerializeField] private string itemWord;
    [SerializeField] private string itemMean;
    [SerializeField] private string itemType;
    private void Awake()
    {
        InitUIName();
    }
    private void InitUIName()
    {
        UIword = transform.Find("Word");
        UImean = transform.Find("Mean");
        Word = UIword.GetComponent<Text>();
        Mean = UImean.GetComponent<Text>();
    }
    public void Refresh(PackageLocalItem packageLocalData, PackagePanel uiParent)
    {
        this.packageLocalData = packageLocalData;
        this.uiParent = uiParent;
        //物品图片获取
        itemID = packageLocalData.id;  // 保存物品 ID
        itemUid = packageLocalData.uid;
        itemWord = packageLocalData.word;
        itemMean = packageLocalData.mean;
        itemType = packageLocalData.type;
        Word.text = itemWord;
        Mean.text = itemMean + "." + itemType;
    }
}

