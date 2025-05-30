using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 64;
    public const float tileSizeHeight = 64;

    InventoryItem[,]  inventoryItemSlot;

    RectTransform rectTransform;

    [SerializeField] int GridId;
    [SerializeField] int gridSizeWidth;
    [SerializeField] int gridSizeHeight;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }
    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    Vector2 positionOnTheGrid=new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }
    public int GetGridID()
    {
        return this.GridId;
    }
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; }//确保访问物体数据存在

        ClearGridReference(toReturn);

        return toReturn;
    }
    private void ClearGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)//寻找可存放物体网格
    {
        int height = gridSizeHeight - itemToInsert.HEIGHT+1;
        int width= gridSizeWidth - itemToInsert.WIDTH+1;
        for (int y=0;y< height; y++)
        {
           for (int x = 0;x < width; x++)
           {
                if(CheckAvailableSpace(x, y, itemToInsert.WIDTH, itemToInsert.HEIGHT)==true)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }

    public bool PlaceItem(InventoryItem inventoryItem,int posX,int posY,ref InventoryItem overlapItem)
    {
        if(BoundryCheck(posX,posY,inventoryItem.WIDTH, inventoryItem.HEIGHT) ==false)
        {
            return false;
        }
        if(OverlapCheck(posX,posY,inventoryItem.WIDTH, inventoryItem.HEIGHT,ref overlapItem)==false)
        {
            overlapItem = null;
            return false;
        }
        if(overlapItem!=null)
        {
            ClearGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }
    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.WIDTH; x++)
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }
        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }
    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem,int posX,int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth / 2 * inventoryItem.WIDTH;
        position.y = -(posY * tileSizeHeight + tileSizeHeight / 2 * inventoryItem.HEIGHT);
        return position;
    }
    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)//检查重叠
    {
        for(int x=0;x<width;x++)
        {
            for (int y = 0;y < height; y++)
            {
                if(inventoryItemSlot[posX+x,posY+y]!=null)
                {
                    if(overlapItem==null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if(overlapItem != inventoryItemSlot[posX + x, posY + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
    private bool CheckAvailableSpace(int posX, int posY, int width, int height)//检查重叠
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                      return false;
                }
            }
        }
        return true;
    }
    bool PositionCheck(int posX,int posY)
    {
        if(posX<0 || posY<0)
        {
            return false;
        }
        if(posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }
        return true;
    }
    public bool BoundryCheck(int posX,int posY,int width,int height)
    {
        if (PositionCheck(posX, posY) == false) { return false; }

        posX += width-1;
        posY += height-1;

        if (PositionCheck(posX, posY) == false) { return false; }
        return true;
    }
}
