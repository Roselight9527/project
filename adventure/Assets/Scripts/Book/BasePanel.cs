using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemove = false;
    protected new string name;

    protected virtual void Awake()
    {
    }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        SetActive(true);
    }

    public virtual void ClosePanel()
    {
        isRemove = true;
        SetActive(false);
        Destroy(gameObject);
        if (BookManager.Instance.panelDict.ContainsKey(name))
        {
            BookManager.Instance.panelDict.Remove(name);
        }
    }
    public virtual void OpenPackagePanel()
    {
        SetActive(true);
    }
}