using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    public ItemData itemData;
    private InventoryController uiParent;

    public int HEIGHT
    {
        get
        {
            if(rotated==false)
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }
    public int WIDTH
    {
        get
        {
            if (rotated == false)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }
    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;
    internal void Set(ItemData itemData,InventoryController uiParent)
    {
        this.itemData = itemData;
        this.uiParent = uiParent;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.width * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    internal void Rotate()
    {
        rotated = !rotated;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }
    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null) { return; }
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetParent(targetGrid.GetComponent<RectTransform>());
        rectTransform.SetAsFirstSibling();
    }
    public int GetItemID()
    {
        return this.itemData.id;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked item with ID: {itemData?.id}");
        if (uiParent != null)
        {
            this.uiParent.chooseUID = this.itemData.id;
            Debug.Log($"ID {itemData.id} assigned to InventoryController.");
        }
        else
        {
            Debug.LogError("uiParent is null!");
        }
    }
}
