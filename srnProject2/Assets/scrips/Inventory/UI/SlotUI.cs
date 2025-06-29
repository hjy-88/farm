using MFarm.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mfarm.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler,IBeginDragHandler, IDragHandler, IEndDragHandler

    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        //[SerializeField] private Image slotHighlight;
        [SerializeField] private Button button;
        [Header("格子类型")]
        public SlotType slotType;

        public bool isSelected;

        public int slotIndex;
        //物品信息    
        public ItemDetails itemDetails;
        public int itemAmount;


        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
        private void Start()
        {
            isSelected = false;
            if (itemDetails==null)
            {
                UpdateEmptySlot();
            }
        }
        /// <summary>
        /// 更新物品槽显示，设置对应物品信息、图标、数量等
        /// </summary>
        /// <param name="item">物品详情数据</param>
        /// <param name="amount">物品数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// 将物品槽更新为空状态，清理显示、重置交互等
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
                EventHandler.CallItemSelectedEvent(itemDetails,isSelected);
            }
            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null) return;
            isSelected = !isSelected;

            //inventoryUI.UpdateSlotHightlight(slotIndex);

            if (slotType == SlotType.Bag)
            {
                //通知物品被选中的状态和信息
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);

            }
        }

            public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();

                //isSelected = true;
                //inventoryUI.UpdateSlotHighlight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            // Debug.Log(eventData.pointerCurrentRaycast.gameObject);

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;

                // 在Player自身背包范围内交换
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
            }

            /*else 
            {
                if (itemDetails.canDropped)
                {
                    //鼠标对应世界地图坐标
                    var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

                    EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
                }
            }*/

        }
    }
}