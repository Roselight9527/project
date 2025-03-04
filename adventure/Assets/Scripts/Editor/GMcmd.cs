using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GMcmd
{
    [MenuItem("GMcmd/读取表格")]
    public static void ReadTable()
    {
        PackageTable packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        foreach (PackageTableItem packageItem in packageTable.DataList)
        {
            Debug.Log(string.Format("[id]:{0},[word]:{1},[mean]:{2}", packageItem.id, packageItem.word, packageItem.mean));
        }
    }

    [MenuItem("GMcmd/创建背包测试数据")]
    public static void CreateLocalPackageData()
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
    [MenuItem("GMcmd/清除背包数据")]
    public static void ClearData()
    {
        PackageLocalData.Instance.Clearitems();
    }
    [MenuItem("GMcmd/num赋值")]
    public static void AssignNumsFromPackageTable()
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
    [MenuItem("GMcmd/读取背包数据")]
    public static void ReadLocalPackageData()
    {
        //读取数据
        List<PackageLocalItem> readItems = PackageLocalData.Instance.LoadPackage();
        foreach (PackageLocalItem item in readItems)
        {
            Debug.Log(string.Format("[word]:{0},[mean]:{1}", item.word, item.mean));
        }
    }
    [MenuItem("GMcmd/获取单词")]
    public static void Getword()
    {
        GameManager.Instance.GetWord(1);
    }
}
