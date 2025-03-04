using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GMcmd
{
    [MenuItem("GMcmd/��ȡ���")]
    public static void ReadTable()
    {
        PackageTable packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        foreach (PackageTableItem packageItem in packageTable.DataList)
        {
            Debug.Log(string.Format("[id]:{0},[word]:{1},[mean]:{2}", packageItem.id, packageItem.word, packageItem.mean));
        }
    }

    [MenuItem("GMcmd/����������������")]
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
    [MenuItem("GMcmd/�����������")]
    public static void ClearData()
    {
        PackageLocalData.Instance.Clearitems();
    }
    [MenuItem("GMcmd/num��ֵ")]
    public static void AssignNumsFromPackageTable()
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
    [MenuItem("GMcmd/��ȡ��������")]
    public static void ReadLocalPackageData()
    {
        //��ȡ����
        List<PackageLocalItem> readItems = PackageLocalData.Instance.LoadPackage();
        foreach (PackageLocalItem item in readItems)
        {
            Debug.Log(string.Format("[word]:{0},[mean]:{1}", item.word, item.mean));
        }
    }
    [MenuItem("GMcmd/��ȡ����")]
    public static void Getword()
    {
        GameManager.Instance.GetWord(1);
    }
}
