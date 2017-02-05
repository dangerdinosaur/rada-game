using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AGB_Inventory : MonoBehaviour
{
	public UIController ui;

	public List<AGB_Item> items = new List<AGB_Item>();


	void Awake()
	{
		ui = FindObjectOfType<UIController>();
	}


	void Start()
	{
		PopulateUISlots();
	}


	void PopulateUISlots()
	{
		for(int i = 0; i < items.Count; i++)
		{
			ui.AddItemToFirstEmptySlot(items[i]);
		}
	}
}
