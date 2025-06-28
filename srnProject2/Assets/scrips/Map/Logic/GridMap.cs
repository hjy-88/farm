using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;
    public GridType gridType;
    private Tilemap currentTilemap;

    private void OnEnable()
    {
        if(!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent < Tilemap >();
            if (mapData != null)
                mapData.tileproperties.Clear();
        }
    }
    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
#if UNITY_EDITOR
            UpdateTileProperties();
            if (mapData != null)
                EditorUtility.SetDirty(mapData);
#endif
        }
    }
    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();

        if(!Application.IsPlaying(this))
        {
            if(mapData!=null)
            {
                Vector3Int startPos = currentTilemap.cellBounds.min;
                Vector3Int endPos = currentTilemap.cellBounds.max;
                for(int x=startPos.x;x<endPos.x;x++)
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));

                        if(tile!=null)
                        {
                            Tileproperty newTile = new Tileproperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue=true
                            };
                            mapData.tileproperties.Add(newTile);
                        }
                    }
                        
                }
            }
            
        }
    }
}
