using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "1/PackageTable", fileName = "packagetable")]
public class PackageTable : ScriptableObject
{
    public List<PackageTableItem> DataList = new List<PackageTableItem>();
}
[System.Serializable]
public class PackageTableItem
{
    public int id;
    public string word;
    public string type;
    public string mean;
}