using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class AGB_Input : MonoBehaviour
{
	#region INPUT EVENTS AND DELEGATES
	public delegate void OnUpdateMousePositionEvent(Vector3 mousePos);
	public static event OnUpdateMousePositionEvent OnUpdateMousePosition;

	public delegate void OnLeftClickEvent(Vector3 mousePos);
	public static event OnLeftClickEvent OnLeftClick;

	public delegate void OnLeftDoubleClickEvent(Vector3 mousePos);
	public static event OnLeftDoubleClickEvent OnLeftDoubleClick;

	public delegate void OnRightClickEvent(Vector3 mousePos);
	public static event OnRightClickEvent OnRightClick;

	public delegate void OnHoverInteractableEvent(GameObject go);
	public static event OnHoverInteractableEvent OnHoverInteractable;

	public delegate void OnHoverNullEvent();
	public static event OnHoverNullEvent OnHoverNull;
	
	public delegate void OnKeyInputEvent(KeyCode kc);
	public static event OnKeyInputEvent OnKeyInput;
	#endregion


	public bool AllowCursorMovement
	{
		get { return _cursorMovementEnabled; }
		set { _cursorMovementEnabled = value; } 
	}

	public bool EnableClicking
	{
		get { return _clickingEnabled; }
		set { _clickingEnabled = value; } 
	}

	private bool _cursorMovementEnabled = false;
	private bool _clickingEnabled = false;



	// Double Clicks
	private float lastClickTime;
	private float catchTime = 0.25f;

	// Camera
	private Camera cam;


	void Awake()
	{
		cam = Camera.main;
	}


	void Update()
	{
		// If player can move cursor...
		if(_cursorMovementEnabled == true)
		{
			if(OnUpdateMousePosition != null)
			{
				OnUpdateMousePosition(Input.mousePosition);
			}

			// Scan for interactables
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 50.0f);
			
			if(hit.collider != null)
			{
				if(hit.collider.tag == "Interactable")
				{
					// Change the cursor to show it can be interacted with.
					if(OnHoverInteractable != null)
					{
						OnHoverInteractable(hit.collider.gameObject);
					}
				}
				else
				{
					if(OnHoverNull != null)
					{
						OnHoverNull();
					}
				}
			}
			else
			{
				if(OnHoverNull != null)
				{
					OnHoverNull();
				}
			}
		}

		// If player can click...
		if(_clickingEnabled == true && EventSystem.current.IsPointerOverGameObject() == false)
		{
			// Left Clicking
			if(Input.GetMouseButtonDown(0))
			{	
				if(Time.time - lastClickTime < catchTime)
				{
					// Double Click
					if(OnLeftDoubleClick != null)
					{
						OnLeftDoubleClick(Input.mousePosition);
					}
				}
				else
				{
					// Single Click
					if(OnLeftClick != null)
					{
						OnLeftClick(Input.mousePosition);
					}
				}

				// Reset timer
				lastClickTime = Time.time;
			}

			if(Input.GetMouseButtonDown(1))
			{
				if(OnRightClick != null)
				{
					OnRightClick(Input.mousePosition);
				}
			}
		}
	
		// Main menu key...
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(OnKeyInput != null)
				OnKeyInput(KeyCode.Escape);
		}

		// Map key...
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			if(OnKeyInput != null)
				OnKeyInput(KeyCode.Tab);
		}
	}
}
