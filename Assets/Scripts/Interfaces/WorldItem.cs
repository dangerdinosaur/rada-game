using UnityEngine;
using System.Collections;

public class WorldItem : MonoBehaviour, ITalkable, IUseable, ILookable
{
	public int itemId;
	public string itemName;
	public string itemDescription;

	public void InitItem()
	{
		// grab info from the database.
	}

	// Talk to item.
	public void Talk()
	{
		print("I can't talk to that.");
	}

	// Look at item.
	public void Look()
	{
		print("It's a " + itemDescription);
	}

	// Use item.
	public void Use()
	{
		print("I can't use that.");
	}
}
