using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // 引入TextMeshPro命名空间

public class WordCell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int itemID;
    [SerializeField] private string itemUid;
    [SerializeField] private string itemWord;
    [SerializeField] private string itemMean;
    private TextMeshProUGUI text;// 修改为TextMeshPro组件
    private PackageLocalItem packageLocalData;
    private PackageTableItem packageTableItem;
    private PackagePanel uiParent;

    private void Awake()
    {
        // 获取TextMeshPro组件的引用
        text = GetComponent<TextMeshProUGUI>();
    }
    private void InitUIName()
    {
    }
    public void Refresh(PackageLocalItem packageLocalData, PackagePanel uiParent)
    {
        // 数据初始化
        this.packageLocalData = packageLocalData;
        this.packageTableItem = GameManager.Instance.GetPackageItemById(packageLocalData.id);
        this.uiParent = uiParent;
        //物品图片获取
        itemID = packageLocalData.id;  // 保存物品 ID
        itemUid = packageLocalData.uid;// 保存物品 Num
        itemWord = packageLocalData.word;
        itemMean = packageLocalData.mean;
        text.text = itemWord;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.uiParent.chooseUID == this.packageLocalData.uid)
            return;
        // 根据点击设置最新的uid -> 进而刷新详情界面
        this.uiParent.chooseUID = this.packageLocalData.uid;
    }
}