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
    public class PackageItemComparer : IComparer<PackageLocalItem>//根据稀有物排列物品
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
    //数据初始化
    public static void CreateLocalPackageData()//创建动态数据
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
    public static void AssignNumsFromPackageTable()//动态数据赋值
    {
        PackageTable packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        // 遍历 packageTable
        foreach (PackageTableItem packageItem in packageTable.DataList)
        {
            // 在 PackageLocalData 中寻找匹配的 ID
            PackageLocalItem localItem = PackageLocalData.Instance.items
                .Find(item => item.id == packageItem.id);

            // 如果找到匹配的 localItem，则赋值 num
            if (localItem != null)
            {
                localItem.type = packageItem.type;
                localItem.word = packageItem.word;
                localItem.mean = packageItem.mean;
            }
        }
        // 最后保存更新后的数据
        PackageLocalData.Instance.SavePackage();
    }
}
