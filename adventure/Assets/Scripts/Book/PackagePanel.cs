using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackagePanel : BasePanel
{
    private Transform UIDetail;
    private Transform UIClose;
    private string _chooseUid;
    public string chooseUID
    {
        get
        {
            return _chooseUid;
        }
        set
        {
            _chooseUid = value;
            RefreshDetail();
        }
    }
    override protected void Awake()
    {
        InitUIName();
        InitClick();
    }
    private void InitUIName()
    {
        UIDetail = transform.Find("Detail");
        UIClose= transform.Find("Close");
    }
    private void InitClick()
    {
        UIClose.GetComponent<Button>().onClick.AddListener(OnClickCloseBagBtn);
    }

    private void OnClickCloseBagBtn()
    {
        BookManager.Instance.ClosePanel(UIConst.PackagePanel);
        Time.timeScale = 1;
    }

    private void RefreshDetail()
    {
        // �ҵ�uid��Ӧ�Ķ�̬����
        PackageLocalItem localItem = GameManager.Instance.GetPackageLocalItemByUId(chooseUID);
        // ˢ���������
        Debug.Log(string.Format("[id]:{0},[word]:{1}", localItem.id, localItem.word));
        UIDetail.GetComponent<PackageDetail>().Refresh(localItem, this);
    }
}
