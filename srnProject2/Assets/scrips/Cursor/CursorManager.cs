using System.Collections;
using System.Collections.Generic;
using MFarm.Map;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed,item;
    private Sprite currentSprite;
    private Image cursorImage;
    private RectTransform cursorCanvas;

    private Camera mainCamera;
    private Grid currentGrid;

    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;
    private bool cursorEnable;
    private bool cursorPositionValid;
    private ItemDetails currentItem;
    private Transform PlayerTransform => FindObjectOfType<Player>().transform;
    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += onAfterSceneLoadedEvent;
        
    }
    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= onAfterSceneLoadedEvent;
        
    }
    
    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);

        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (cursorCanvas == null) return;
        cursorImage.transform.position = Input.mousePosition;
        if (!InteractWithUI()&&cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
            SetCursorImage(normal);
    }
    private void CheckPlayerInput()
    {
        if(Input.GetMouseButtonDown(0)&&cursorPositionValid)
        {
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currentItem);
        }
    }
    private void onAfterSceneLoadedEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        //cursorEnable = true;
        /*if (currentGrid == null)
        {
            Debug.LogError("找不到Grid组件！请确保场景中有Grid组件");
            
        }*/
    }
    
    private void OnBeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);

    }
    private void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    private void SetCursorInvalid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.5f);
    }
    private void OnItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        
        if(!isSelected)
        {
            currentItem = null;
            cursorEnable = false;
            currentSprite = normal;
        }
        else
        {
            currentItem = itemDetails;
            
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed=>seed,
                ItemType.HoeTool=>tool,
                ItemType.ChopTool => tool,
                ItemType.WaterTool => tool,
                ItemType.CollectTool => tool,
                ItemType.Commodity=>item,
                _=>normal
            };
            cursorEnable = true;
        }
        
    }
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        //Debug.Log("WorldPos:" + mouseWorldPos + " GridPos:" + mouseGridPos);

        var playerGridPos = currentGrid.WorldToCell(PlayerTransform.position);
        if(Mathf.Abs(mouseGridPos.x-playerGridPos.x)>currentItem.itemUseRadius||Mathf.Abs(mouseGridPos.y-playerGridPos.y)>currentItem.itemUseRadius)
        {
            SetCursorInvalid();
            return;
        }
        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
        if(currentTile!=null)
        {
            switch(currentItem.itemType)
            {
                case ItemType.Commodity:
                    if (currentTile.canDropItem&&currentTile.canDropItem) SetCursorValid(); else SetCursorInvalid();
                    break;
                case ItemType.HoeTool:
                    if (currentTile.canDig) SetCursorValid();
                    else SetCursorInvalid();
                    break;
                case ItemType.WaterTool:
                    if ((currentTile.daySinceDug > -1 && currentTile.daysSinceWatered == -1)) SetCursorValid(); else SetCursorInvalid();
                    break;
            }

        }
        else
        {
            SetCursorInvalid();

        }
        //Debug.Log("WorldPos:" + mouseWorldPos + " GridPos:" + mouseGridPos);
    }
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return true;
        return false;
    }
}
