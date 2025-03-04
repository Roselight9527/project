using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // ����TextMeshPro�����ռ�

public class WordCell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int itemID;
    [SerializeField] private string itemUid;
    [SerializeField] private string itemWord;
    [SerializeField] private string itemMean;
    private TextMeshProUGUI text;// �޸�ΪTextMeshPro���
    private PackageLocalItem packageLocalData;
    private PackageTableItem packageTableItem;
    private PackagePanel uiParent;

    private void Awake()
    {
        // ��ȡTextMeshPro���������
        text = GetComponent<TextMeshProUGUI>();
    }
    private void InitUIName()
    {
    }
    public void Refresh(PackageLocalItem packageLocalData, PackagePanel uiParent)
    {
        // ���ݳ�ʼ��
        this.packageLocalData = packageLocalData;
        this.packageTableItem = GameManager.Instance.GetPackageItemById(packageLocalData.id);
        this.uiParent = uiParent;
        //��ƷͼƬ��ȡ
        itemID = packageLocalData.id;  // ������Ʒ ID
        itemUid = packageLocalData.uid;// ������Ʒ Num
        itemWord = packageLocalData.word;
        itemMean = packageLocalData.mean;
        text.text = itemWord;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.uiParent.chooseUID == this.packageLocalData.uid)
            return;
        // ���ݵ���������µ�uid -> ����ˢ���������
        this.uiParent.chooseUID = this.packageLocalData.uid;
    }
}