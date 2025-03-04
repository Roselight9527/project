using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }
    public void SetSize(InventoryItem targetItem)
    {
        if (targetItem == null)
        {
            Debug.LogWarning("目标物品为空，无法设置高亮大小。");
            return;
        }
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth;
        size.y = targetItem.HEIGHT * ItemGrid.tileSizeHeight;
        highlighter.sizeDelta = size;
    }
    public void SetPosition(ItemGrid targetGrid,InventoryItem targetItem)
    {
        SetParent(targetGrid);

        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY);
        highlighter.localPosition = pos;
    }
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem,int posX,int posY)
    {
      Vector2 pos=targetGrid.CalculatePositionOnGrid
           (
            targetItem,
            posX,
            posY
        );
        highlighter.localPosition = pos;
    }
    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null) { return; }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
        highlighter.SetAsFirstSibling();
    }
}
