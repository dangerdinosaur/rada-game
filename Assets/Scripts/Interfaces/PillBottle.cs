using UnityEngine;
using System.Collections;

public class PillBottle : MonoBehaviour, ITalkable
{
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			Talk();
		}
	}

	public void Talk()
	{
		print("I'm a pill bottle, what about you?");
	}
}
