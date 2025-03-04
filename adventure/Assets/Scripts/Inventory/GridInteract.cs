using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    InventoryController inventoryController;
    ItemGrid itemGrid;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)//鼠标进入时将selectedItemGrid属性设置为当前itemGrid
    {
        Debug.Log("enter");
        inventoryController.SelectedItemGrid = itemGrid;
    }
    public void OnPointerExit(PointerEventData eventData)//鼠标退出
    {
        Debug.Log("exit");
        inventoryController.SelectedItemGrid = null;
    }
}
