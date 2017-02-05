using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;


public class ClickManager : MonoBehaviour
{
	public GameObject customCursor;

	private RectTransform ccRectTransform;
	private Image ccImage;


	void Awake()
	{
		// Custom cursor references.
//		ccRectTransform = customCursor.GetComponent<RectTransform>();
//		ccImage = customCursor.GetComponent<Image>();
//		ccImage.color = Color.grey;

		// Hide the default Unity cursor.
//		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		/*
		if(EventSystem.current.IsPointerOverGameObject())
		{
			ccImage.color = Color.white;
		}
		else
		{
			// Default cursor color.
			ccImage.color = Color.grey;
		}

		// Track the custom cursor to the mouse position.
		ccRectTransform.position = Input.mousePosition;
		*/
	}
}
