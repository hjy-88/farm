using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDetails///详情
{
    public int itemID;///物品id查询
    public string itemName;///物品名字itemName
    public ItemType itemType;///物品类型
    public Sprite itemIcon;///物品图标
    public Sprite itemOnWorldSprite;///物品在世界地图产生的图片
    public string itemDescription;///详情
    public int itemUseRadius;///物品可使用范围

    ///物品状态
    public bool canPickedup;///拾取
    public bool canDropped;///丢弃
    public bool canCarried;///举着
    public int itemPrice;///价值
    [Range(0, 1)]
    public float sellPercentage;///售卖的折扣
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