using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MapLocation : MonoBehaviour, IPointerClickHandler
{
	public int locationId;
	private UIController ui;


	void Awake()
	{
		ui = FindObjectOfType<UIController>();
	}


	// Click events - left click to look at / right click to move location
	public void OnPointerClick(PointerEventData data)
	{
		if(data.button == PointerEventData.InputButton.Left)
		{
			print("It's map location: " + locationId);
		}
		else if(data.button == PointerEventData.InputButton.Right)
		{
			print("Player moving to map location: " + locationId);
		}
	}
}