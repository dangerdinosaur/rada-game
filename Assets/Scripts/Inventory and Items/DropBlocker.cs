using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DropBlocker : MonoBehaviour, IDropHandler
{
	private UIController ui;


	void Awake()
	{
		ui = FindObjectOfType<UIController>();
	}


	public void OnDrop(PointerEventData data)
	{
		// Only run if the player is dragging.
		if(ui.isDragging)
		{
			// End the drag.
			ui.CancelDrag();
		}
	}
}
