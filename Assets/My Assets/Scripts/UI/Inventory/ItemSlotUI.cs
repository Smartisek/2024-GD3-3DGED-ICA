using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData ItemData { get; private set; }
    public int ItemCount { get; private set; }

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;

    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void SetItem(ItemData itemData, int count)
    {
        ItemData = itemData;
        ItemCount = count;
        itemIcon.sprite = itemData.ItemIcon;
        itemName.text = itemData.ItemName;
        itemCount.text = count.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter != null)
        {
            InventoryUI targetInventoryUI = eventData.pointerEnter.GetComponentInParent<InventoryUI>();
            if (targetInventoryUI != null)
            {
                targetInventoryUI.HandleItemDrop(this);
            }
        }
    }
}