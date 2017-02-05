using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class Slot : MonoBehaviour, IPointerDownHandler, IDragHandler, IDropHandler
{
	public int id;
	public AGB_Item curItem;

	private Image slotImage;
	private Color slotColor;
	private Sprite emptySlotSprite;
	private UIController ui;


	void Awake()
	{
		slotImage = GetComponent<Image>();
		emptySlotSprite = slotImage.sprite;
		slotColor = slotImage.color;
		ui = FindObjectOfType<UIController>();
//		slotImage.enabled = false;
		// Hide image by default.
		ShowImage(false);
	}
	
	// Click event
	public void OnPointerDown(PointerEventData data)
	{
//		print("Clicked down on " + id);

		if(curItem != null)
		{
			ui.SelectItem(curItem, id);
		}
	}

	public void OnDrag(PointerEventData data)
	{
		// Only run if there is an item on the slot.
		if(curItem != null)
		{
//			print("Dragging on slot " + id);

			ui.StartDrag(curItem, id);
			// Remove the dragged item from its previous slot.
			ui.RemoveItemFromSlot(id);
		}
	}


	public void OnDrop(PointerEventData data)
	{
		// Only run if the player is dragging.
		if(ui.isDragging)
		{
//			print("Dropped on slot" + id);

			// If there's no item in the slot.
			if(curItem == null)
			{
				// Add the item to this slot.
				ui.AddItemToSlot(id, ui.draggedItem);
			}
			// If there is an item in the slot.
			else if(curItem != null)
			{
				// Swap the items.
				// Add the current item to the original dragged slot id.
				ui.AddItemToSlot(ui.draggedItemStartingSlotId, curItem);
				// Add the dragged item to this slot id.
				ui.AddItemToSlot(id, ui.draggedItem);
			}

			// Select the current item.
			ui.SelectItem(curItem, id);
			// End the drag.
			ui.EndDrag();
		}
	}


	public void AddItemToSlot(AGB_Item item)
	{
		curItem = item;
		slotImage.sprite = item.iconSprite;
//		slotImage.enabled = true;
		ShowImage(true);
	}


	public void RemoveItemFromSlot()
	{
		curItem = null;
		slotImage.sprite = emptySlotSprite;
//		slotImage.enabled = false;
		ShowImage(false);
	}


	public void ShowImage(bool enable)
	{
		if (enable == true) {
			slotColor.a = 1f;
		} else if (enable == false) {
			slotColor.a = 0f;
		}

		slotImage.color = slotColor;
	}
}
