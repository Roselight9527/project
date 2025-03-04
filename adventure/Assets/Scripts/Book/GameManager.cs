using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private PackageTable packageTable;
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void Start()
    {
        //CreateLocalPackageData();
        //AssignNumsFromPackageTable();
    }
    private void OnDestroy()
    {
        PackageLocalData.Instance.Clearitems();
    }
    public PackageTable GetPackageTable()
    {
        if (packageTable == null)
        {
            packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        }
        return packageTable;
    }

    public List<PackageLocalItem> GetPackageLocalData()
    {
        return PackageLocalData.Instance.LoadPackage();
    }
    public PackageTableItem GetPackageItemById(int id)
    {
        List<PackageTableItem> packageDataList = GetPackageTable().DataList;
        foreach (PackageTableItem item in packageDataList)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public PackageLocalItem GetPackageLocalItemByUId(string uid)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        foreach (PackageLocalItem item in packageDataList)
        {
            if (item.uid == uid)
            {
                return item;
            }
        }
        return null;
    }
    public PackageLocalItem GetPackageLocalItemById(int id)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        foreach (PackageLocalItem item in packageDataList)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public PackageLocalItem GetPackageLocalItemByword(string word)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        foreach (PackageLocalItem item in packageDataList)
        {
            if (item.word == word)
            {
                return item;
            }
        }
        return null;
    }
    public void GetWord(int index)
    {
        if (index==0)
        {
            return;
        }
        if (!PackageLocalData.Instance.itemsid.Contains(index))
        {
            List<PackageTableItem> packageItems = GetPackageTable().DataList;
            PackageTableItem packageItem = packageItems[index];
            PackageLocalItem packageLocalItem = new()
            {
                id = packageItem.id,
                uid = System.Guid.NewGuid().ToString(),
                word = packageItem.word,
                type = packageItem.type,
                mean = packageItem.mean
            };
            PackageLocalData.Instance.itemsid.Add(index);
            PackageLocalData.Instance.items.Add(packageLocalItem);
            PackageLocalData.Instance.SavePackage();
        }
    }
    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = PackageLocalData.Instance.LoadPackage();
        //localItems.Sort(new PackageItemComparer());
        return localItems;
    }
    public int GetItemnum()
    {
        List<PackageLocalItem> localItems = PackageLocalData.Instance.LoadPackage();
        return localItems.Count;
    }
    public List<PackageLocalItem> GetAddPackageLocalData()
    {
        List<PackageLocalItem> localItems = PackageLocalData.Instance.LoadAdditems();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }
    public class PackageItemComparer : IComparer<PackageLocalItem>//����ϡ����������Ʒ
    {
        public int Compare(PackageLocalItem a, PackageLocalItem b)
        {
            PackageTableItem x = GameManager.Instance.GetPackageItemById(a.id);
            PackageTableItem y = GameManager.Instance.GetPackageItemById(b.id);
            int typeComparison = y.type.CompareTo(x.type);

            if (typeComparison == 0)
            {
                int idComparison = y.id.CompareTo(x.id);
                return idComparison;
            }
            return typeComparison;
        }
    }
    //���ݳ�ʼ��
    public static void CreateLocalPackageData()//������̬����
    {
        PackageTable packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        PackageLocalData.Instance.items = new List<PackageLocalItem>();
        for (int i = 1; i < packageTable.DataList.Count; i++)
        {
            PackageLocalItem packageLocalItem = new()
            {
                uid = Guid.NewGuid().ToString(),
                id = i,
            };
            PackageLocalData.Instance.items.Add(packageLocalItem);
        }
        PackageLocalData.Instance.SavePackage();

        //List<PackageLocalItem> readItems = PackageLocalData.Instance.LoadPackage();
    }
    public static void AssignNumsFromPackageTable()//��̬���ݸ�ֵ
    {
        PackageTable packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        // ���� packageTable
        foreach (PackageTableItem packageItem in packageTable.DataList)
        {
            // �� PackageLocalData ��Ѱ��ƥ��� ID
            PackageLocalItem localItem = PackageLocalData.Instance.items
                .Find(item => item.id == packageItem.id);

            // ����ҵ�ƥ��� localItem����ֵ num
            if (localItem != null)
            {
                localItem.type = packageItem.type;
                localItem.word = packageItem.word;
                localItem.mean = packageItem.mean;
            }
        }
        // ��󱣴���º������
        PackageLocalData.Instance.SavePackage();
    }
}
