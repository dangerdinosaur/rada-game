using UnityEngine;
using System.Collections;


public interface IInteractable
{
	void Interact();
}

public interface ILookable
{
	void Look();
}

public interface IUseable
{
	void Use();
}

public interface ITalkable
{
	void Talk();
}

public interface IPickupable
{
	void PickUp();
}