using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed,item;
    private Sprite currentSprite;
    private Image cursorImage;
    private RectTransform cursorCanvas;
    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
    }
    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
    }
    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);
    }
    private void Update()
    {
        if (cursorCanvas == null) return;
        cursorImage.transform.position = Input.mousePosition;
        if (!InteractWithUI())
            SetCursorImage(currentSprite);
        else
            SetCursorImage(normal);
    }
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);

    }
    private void OnItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        if(!isSelected)
        {
            currentSprite = normal;
        }
        else
        {
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
        }
        
    }
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return true;
        return false;
    }
}
