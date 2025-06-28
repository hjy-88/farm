using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDetails///����
{
    public int itemID;///��Ʒid��ѯ
    public string itemName;///��Ʒ����itemName
    public ItemType itemType;///��Ʒ����
    public Sprite itemIcon;///��Ʒͼ��
    public Sprite itemOnWorldSprite;///��Ʒ�������ͼ������ͼƬ
    public string itemDescription;///����
    public int itemUseRadius;///��Ʒ��ʹ�÷�Χ

    ///��Ʒ״̬
    public bool canPickedup;///ʰȡ
    public bool canDropped;///����
    public bool canCarried;///����
    public int itemPrice;///��ֵ
    [Range(0, 1)]
    public float sellPercentage;///�������ۿ�
}

[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}

[System.Serializable]
public class AnimatorType
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}

[System.Serializable]
public class Tileproperty
{
    public Vector2Int tileCoordinate;
    public GridType gridType;
    public bool boolTypeValue;
}

[System.Serializable]
public class TileDetails
{
    public int gridX, gridY;
    public bool canDig;
    public bool canDropItem;
    public bool isNPCObstacle;
    public int daySinceDug = -1;
    public int daysSinceWatered = -1;
    public int seedItemId = -1;
    public int growthDays = -1;
    public int daysSinceLastHarvset = -1;
}