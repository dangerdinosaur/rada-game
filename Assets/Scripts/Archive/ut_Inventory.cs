using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This is just a simple inventory example to show you how to integrate Untwiner with your own inventory script.
public class ut_Inventory : MonoBehaviour
{
	// Generic Item class.
	public class Item
	{
		public string itemName;				// Name
		public int itemValue;				// Value
	}

	// Dictionary to hold the items, keyed by their names.
	public Dictionary<string, Item> items = new Dictionary<string, Item>();

/*
	// In start - add some generic items that work with the example scenes. 
	void Start()
	{
		// Define some items.
		Item blueKey = new Item { itemName = "BlueKey", itemDescription = "The Blue Key, like from Doom.", itemValue = 1 };
		Item redKey = new Item { itemName = "RedKey", itemDescription = "The Red Key, also like from Doom.", itemValue = 1 };

		// Add them to the items Dictionary.
		keys.Add(blueKey.itemName, blueKey);
		keys.Add(redKey.itemName, redKey);
	}
*/
	public void AddItem(string iName, int iValue)
	{
		if(hasItem(iName) == true)
		{
			// Item already exists.
			return;
		}
		else
		{
			// Create a new item and add it to the list.
			Item newItem = new Item { itemName = iName, itemValue = iValue };
			items.Add(newItem.itemName, newItem);

			// Send event to GUI to show notification.
			ut_EventManager.AddItem(iName);
		}
	}

	// Check for an item by name.
	public bool hasItem(string itemName)
	{
		if(items.ContainsKey(itemName))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}