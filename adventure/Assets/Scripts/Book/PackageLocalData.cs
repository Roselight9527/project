using UnityEngine;
using System.Collections.Generic;

public class PackageLocalData
{
    private static PackageLocalData _instance;

    public static PackageLocalData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PackageLocalData();
                _instance.itemsid = new List<int>();
                _instance.items = new List<PackageLocalItem>();
            }
            return _instance;
        }
    }


    public List<PackageLocalItem> items;
    public List<int> itemsid;

    public List<PackageLocalItem> additems;

    public void SavePackage()
    {
        string inventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PackageLocalData", inventoryJson);
        PlayerPrefs.Save();
    }
    public List<PackageLocalItem> LoadAdditems()
    {
        if (additems != null)
        {
            return additems;
        }
        additems = new List<PackageLocalItem>();
        return additems;
    }
    public void Clearitems()
    {
        items.Clear();
        SavePackage();
    }
    public void ClearAdditems()
    {
        additems.Clear();
    }
    public List<PackageLocalItem> LoadPackage()
    {
        if (items != null)
        {
            return items;
        }
        if (PlayerPrefs.HasKey("PackageLocalData"))
        {
            string inventoryJson = PlayerPrefs.GetString("PackageLocalData");
            PackageLocalData packageLocalData = JsonUtility.FromJson<PackageLocalData>(inventoryJson);
            items = packageLocalData.items;
            return items;
        }
        else
        {
            items = new List<PackageLocalItem>();
            return items;
        }
    }
}


[System.Serializable]
public class PackageLocalItem
{
    public int id;
    public string uid;
    public string word;
    public string type;
    public string mean;
}
