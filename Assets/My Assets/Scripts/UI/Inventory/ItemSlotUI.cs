using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables
    public ItemData ItemData { get; private set; } //give access
    public int ItemCount { get; private set; } //give access

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;

    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    #endregion

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>(); //get the canvas
        canvasGroup = GetComponent<CanvasGroup>(); //get the canvas group even if it is the same canvas
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>(); //add canvas group if not present
        }
    }

    public void SetItem(ItemData itemData, int count) //set the item slot with corresponding data and count 
    {
        ItemData = itemData;
        ItemCount = count;
        itemIcon.sprite = itemData.ItemIcon;
        itemName.text = itemData.ItemName;
        itemCount.text = count.ToString();
    }

    #region DRAG AND DROP LOGIC
    //using unity interface for drag and drop with help of copilot 
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; //store the original parent
        transform.SetParent(canvas.transform); //set the parent to canvas
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //set the position of the item slot to the mouse position
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent); //set the parent back to the original parent
        canvasGroup.blocksRaycasts = true; 

        if (eventData.pointerEnter != null) 
        {
            InventoryUI targetInventoryUI = eventData.pointerEnter.GetComponentInParent<InventoryUI>(); //get the inventory UI of the target
            if (targetInventoryUI != null)
            {
                targetInventoryUI.HandleItemDrop(this); //handle the item drop
            }
        }
    }
    #endregion
}