using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFarm.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        public List<MapData_SO> mapDataList;
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();
        private Grid currentGrid;
        private void OnEnable()
        {
            EventHandler.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        }
        private void OnDisable()
        {
            EventHandler.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        }
        private void Start()
        {
            foreach(var mapData in mapDataList)
            {
                InitTileDetailsDict(mapData);
            }
        }
        private void InitTileDetailsDict(MapData_SO mapData)
        {
            foreach(Tileproperty tileProperty in mapData.tileproperties)
            {
                TileDetails tileDetails = new TileDetails
                {
                    gridX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };
                string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;

                if(GetTileDetails(key)!=null)
                {
                    tileDetails = GetTileDetails(key);
                }

                switch(tileProperty.gridType)
                {
                    case GridType.diggable:
                        tileDetails.canDig = tileProperty.boolTypeValue;
                        break;
                    case GridType.Dropitem:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPCObstacle:
                        tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                        break;
                }
                if (GetTileDetails(key) != null)
                    tileDetailsDict[key] = tileDetails;
                else
                    tileDetailsDict.Add(key, tileDetails);
            }
        }
        private TileDetails GetTileDetails(string key)
        {
            if (tileDetailsDict.ContainsKey(key))
                return tileDetailsDict[key];
            return null;
        }
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            //Debug.Log($"²éÑ¯ÍßÆ¬¼ü: {key}");
            //Debug.Log($"×Öµä°üº¬¸Ã¼ü: {tileDetailsDict.ContainsKey(key)}");
            return GetTileDetails(key);
        }
        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
        }
        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos,ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

            if(currentTile!=null)
            {
                switch(itemDetails.itemType)
                {
                    case ItemType.Commodity:
                        EventHandler.CallInstantiateItemInScene(itemDetails.itemID, mouseWorldPos);
                        break;
                }
            }
        }
    }
}

