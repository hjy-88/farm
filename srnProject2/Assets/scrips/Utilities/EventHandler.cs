using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class EventHandler
{
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location, list);
    }

    public static event Action<int, Vector3> InstantiateItemInScene;//在地面上生成物品
    public static void CallInstantiateItemInScene(int ID, Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID, pos);
    }
    public static event Action<int, int, int, int, int,int,Season> GameTimeEvent;
    public static void CallGameTimeEvent(int second, int minute, int hour, int day, int month, int year, Season season)
    {
        GameTimeEvent?.Invoke(second, minute, hour, day, month, year, season);
    }
}