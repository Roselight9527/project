using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;
    
    public ItemGrid SelectedItemGrid//初始化成员属性
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }
    [SerializeField] private int _chooseUid;
    public int chooseUID
    {
        get
        {
            return _chooseUid;
        }
        set
        {
            _chooseUid = value;
        }
    }
    [SerializeField] InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;
    bool isinsert=false;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;
    [SerializeField] ItemGrid InsertGrid;

    [Header("广播")]
    public EquipmentEventSO equipmentEvent;

    InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }
    private void Update()
    {
        ItemIconDrag();
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            if(selectedItem==null)
            {
                CreateRandomItem();
            }
            if(selectedItemGrid==null)
            {
                inventoryHighlight.Show(false);
                return;
            }
        }
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            isinsert = true;
            InsertRandomItem();
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RotateItem();
        }
        if (selectedItemGrid == null) 
        {
            inventoryHighlight.Show(false);
            return;
        }
        if (selectedItemGrid != null && !isinsert)
        {
            HandleHighlight();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LeftMouseButtonPress();
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null) { return; }

        selectedItem.Rotate();
    }

    private void InsertRandomItem()
    {
        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        selectedItemGrid = InsertGrid;
        if (selectedItemGrid == null) { return; }
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
        if (posOnGrid == null) { return; }
        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        isinsert = false;
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) { return; }
        oldPosition = positionOnGrid;
        if(selectedItem==null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if(itemToHighlight!=null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                //inventoryHighlight.SetParent(selectedItemGrid);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }else
            {
                inventoryHighlight.Show(false);
            }
        }else
        {
            //边界检查，越界不显示高亮
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.WIDTH,
                selectedItem.HEIGHT
                )) ;
            inventoryHighlight.SetSize(selectedItem);
            //inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem,positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()//实例化
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID],this);
        Image imageComponent = inventoryItem.GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.SetNativeSize();
        }
    }
    private void ItemIconDrag()//可视化
    {
        if (selectedItem != null)
        {
            rectTransform.position = Mouse.current.position.ReadValue();//将鼠标坐标赋值给物体实现物体可视化移动
        }
    }
    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Mouse.current.position.ReadValue();

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }
        return selectedItemGrid.GetTileGridPosition(position);
    }
    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition=GetTileGridPosition();
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }
    private void PlaceItem(Vector2Int tileGridPosition)//放下物体
    {
        bool complete=selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y,ref overlapItem);
        if(complete)
        {
            if(selectedItem!=null && selectedItemGrid.GetGridID()==1)
            {
                equipmentEvent.RaiseEvent(items[selectedItem.GetItemID()], true);
            }
            selectedItem = null;
            if(overlapItem!=null)
            {
                if( selectedItemGrid.GetGridID() == 1)
                {
                equipmentEvent.RaiseEvent(items[overlapItem.GetItemID()], false);
                }
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
    }
    private void PickUpItem(Vector2Int tileGridPosition)//拿起物体
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            if(selectedItemGrid.GetGridID() == 1)
            {
            equipmentEvent.RaiseEvent(items[selectedItem.GetItemID()], false);
            }
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }
}
