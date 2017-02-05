using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
	// Canvas and Panels
	[Header("CANVAS AND PANELS")]
	public Canvas canvas = null;
	public GameObject mainMenu = null;
	public GameObject chapterSpot = null;
	public GameObject inGameMenu = null;
	public GameObject mapPanel = null;
	public GameObject conversationPanel = null;
	public GameObject pauseMenu = null;
	[Space(Utilities.editorSpacerHeight)]

	// Conversations
	[Header("CONVERSATION ELEMENTS")]
	public GameObject responseContainer;
	public GameObject dialogueBox;
	[Space(Utilities.editorSpacerHeight)]

	// In Game
	[Header("IN-GAME UI")]
	public RectTransform mouseCursor;
	public Text hintTextBox;
	public RectTransform labelPrefab;
	[Space(Utilities.editorSpacerHeight)]

	// DEV MODE
	[Header("DEV MODE")]
	public bool skipIntro;
	[Space(Utilities.editorSpacerHeight)]

	// Canvas and Panels
	private bool pauseMenuOpen;
	private bool mapOpen;
	private CanvasGroup mainMenuCanvasGroup;
	private CanvasGroup chapterSpotCanvasGroup;
	private float mainMenuFadeTime = 2.5f;
	private float chapterSpotFadeTime = 1.5f;

	// Mouse
	private Vector3 mouseOriginalLocalScale;
	private Vector3 mouseActiveLocalScale;

	// Conversations
	private CanvasGroup responseCanvasGroup;
	private Text[] optionsTextBoxes;
	private Text dialogueBoxText;
	private int responseNumber;
	public Color playerConvoColor;
	private Color npcConvoColor;
	
	// Slots
	[Header("INVENTORY")]
	public Slot slotPrefab;
	public Image dragImagePrefab;
	public RectTransform itemGridRect;
	
	[HideInInspector]
	public bool isDragging = false;				// Is the player dragging an item?
	[HideInInspector]
	public int draggedItemStartingSlotId;		// The id of the dragged item's slot at the start of the drag.
	[HideInInspector]
	public AGB_Item draggedItem;					// The dragged item.
	
	// Private refs
	private Image dragImage;
	private List<Slot> slots = new List<Slot>();		// A list of the slots.
	private int xPos = -417;		// Inventory UI x position. -96
	private int yPos = 50;			// Inventory UI y position. Half the bottom bar height.
	private int columns = 12;		// Number of columns.
	private int rows = 1;			// Number of rows.
	private int slotSize = 64;		// Size of the slot (resumed 1:1 square ratio)
	private int slotPadding = 6;
	private AGB_Item selectedItem;
	private int selectedItemSlotId;
	private Camera cam;
	public LayerMask interactableMask;
	private GameMaster gm;


	void OnEnable()
	{
		AGB_Input.OnUpdateMousePosition += UpdateCustomCursorPosition;
		AGB_Input.OnHoverInteractable += MouseOverActiveArea;
		AGB_Input.OnHoverNull += MouseOverInactiveArea;
		AGB_Input.OnKeyInput += KeyInputs;
	}
	
	
	void OnDisable()
	{
		AGB_Input.OnUpdateMousePosition -= UpdateCustomCursorPosition;
		AGB_Input.OnHoverInteractable -= MouseOverActiveArea;
		AGB_Input.OnHoverNull -= MouseOverInactiveArea;
		AGB_Input.OnKeyInput -= KeyInputs;
	}


	void Awake()
	{
		// HACK
		OpenConversationPanel();

		// Panels
		mainMenuCanvasGroup = mainMenu.GetComponent<CanvasGroup>();
		chapterSpotCanvasGroup = chapterSpot.GetComponent<CanvasGroup>();

		// Mouse
		mouseOriginalLocalScale = mouseCursor.transform.localScale;
		mouseActiveLocalScale = mouseOriginalLocalScale * 1.5f;

		// Conversations
		dialogueBoxText = dialogueBox.GetComponentInChildren<Text>();
		optionsTextBoxes = responseContainer.GetComponentsInChildren<Text>();
		responseCanvasGroup = responseContainer.GetComponentInChildren<CanvasGroup>();

		// Get References
		gm = FindObjectOfType<GameMaster>();
		cam = Camera.main;

		// Generate the inventory grid.
		GenerateSlotGrid();
		
		// Init UI
		// Conversations
		ClearDialogueBoxText();
		ClearOptions();

		// Init the menus.
		if(skipIntro)
		{
			HideLabel();
			CloseMainMenu();
			CloseChapterSpot();
			CloseConversationPanel();
			ClosePauseMenu();
			CloseMapPanel();
			OpenInGameMenu();
		}
		else
		{
			HideLabel();
			CloseChapterSpot();
			CloseConversationPanel();
			ClosePauseMenu();
			CloseInGameMenu();
			CloseMapPanel();
			OpenMainMenu();
		}

		// Hide the default mouse cursor. 
		Cursor.visible = false;
	}


	public void KeyInputs(KeyCode kc)
	{
		switch(kc)
		{
		case KeyCode.Escape:
			TogglePauseMenu();
			break;
		case KeyCode.Tab:
			ToggleMapPanel();
			break;
		default:
			break;
		}
	}


	// Update is called once per frame
	void Update()
	{
		#region DRAG AND DROP
		// Move icon with mouse.
		if(dragImage != null && isDragging)
		{
			// Drag image
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
			dragImage.transform.position = canvas.transform.TransformPoint(pos);
		}
		
		// Mouse up while dragging special cases.
		if(Input.GetMouseButtonUp(0) && isDragging)
		{
			// If the mouse is over an interactable, drop the draggeditem on the interactable!
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, interactableMask);
			
			if(hit.collider != null)
			{
				if(hit.collider.tag == "Interactable")
				{
					Interactable target = hit.collider.GetComponent<Interactable>();
					
					if(target != null)
					{	
						// Try to use the dragged item on the interactable.
						if(target.CanUseWith(draggedItem.itemId))
						{
//							gm.StartUseWithSequence(target, draggedItem.itemId);
							
							// End the drag.
							EndDrag();
							
							return;
						}
						
						CancelDrag();
						
						return;
					}
				}
			}
			
			// If mouse up over the game world.
			if(EventSystem.current.IsPointerOverGameObject() == false)
			{
				CancelDrag();
			}
		}
		#endregion
	}

	#region PANELS
	public void OpenMainMenu()
	{
		mainMenu.SetActive(true);
	}


	public void CloseMainMenu()
	{
		mainMenu.SetActive(false);
	}


	public void OpenChapterSpot()
	{
		chapterSpot.SetActive(true);
	}
	
	
	public void CloseChapterSpot()
	{
		chapterSpot.SetActive(false);
	}


	public void OpenInGameMenu()
	{
		inGameMenu.SetActive(true);
	}
	
	
	public void CloseInGameMenu()
	{
		inGameMenu.SetActive(false);
	}


	public void OpenMapPanel()
	{
		mapPanel.SetActive(true);
		mapOpen = true;
	}
	
	
	public void CloseMapPanel()
	{
		mapPanel.SetActive(false);
		mapOpen = false;
	}


	public void ToggleMapPanel()
	{
		if(mapOpen == false)
		{
			// Open it
			OpenMapPanel();
		}
		else if(mapOpen == true)
		{
			// Close it
			CloseMapPanel();
		}
	}

	// Chapter start
	IEnumerator FadeOut()
	{
		// Pause for a bit.
		yield return new WaitForSeconds(0.2f);

		// Fade the alpha of the menu out.
		for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / mainMenuFadeTime)
		{
			mainMenuCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
			yield return null;
		}

		// Deactive the menu.
		CloseMainMenu();

		// Pause for a bit.
		yield return new WaitForSeconds(0.5f);

		OpenChapterSpot();

		// Pause for a bit.
		yield return new WaitForSeconds(3.5f);

		// Fade the alpha of the chapter spot UI element out.
		for(float t2 = 0.0f; t2 < 1.0f; t2 += Time.deltaTime / chapterSpotFadeTime)
		{
			chapterSpotCanvasGroup.alpha = Mathf.Lerp(1, 0, t2);
			yield return null;
		}

		CloseChapterSpot();

		OpenInGameMenu();
	}
	

	public void ClickToStartGame()
	{
		StartCoroutine(FadeOut());
	}


	public void OpenPauseMenu()
	{
		Time.timeScale = 0;
		pauseMenu.SetActive(true);
		pauseMenuOpen = true;
	}


	public void ClosePauseMenu()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1;
		pauseMenuOpen = false;
	}

	public void TogglePauseMenu()
	{
		if(pauseMenuOpen == false)
		{
			// Open it
			OpenPauseMenu();
		}
		else if(pauseMenuOpen == true)
		{
			// Close it
			ClosePauseMenu();
		}
	}
	#endregion

	#region IN-GAME UI
	public void UpdateCustomCursorPosition(Vector3 mousePos)
	{
		// Custom Mouse Cursor 
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePos, canvas.worldCamera, out pos);
		mouseCursor.position = canvas.transform.TransformPoint(pos);
	}

	public void MouseOverActiveArea(GameObject go)
	{
		// Widen the object by 0.1
//		mouseCursor.localScale = mouseActiveLocalScale;
	}
	
	public void MouseOverInactiveArea()
	{
//		mouseCursor.transform.localScale = mouseOriginalLocalScale;
	}

	public void SetHintSentence(string str)
	{
		hintTextBox.text = str;
	}

	public void DisplayLabel(Vector3 targetPos, string labelText)
	{
		// Label
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, RectTransformUtility.WorldToScreenPoint(cam, targetPos), canvas.worldCamera, out pos);
		pos = new Vector2(pos.x, pos.y - 15);

		labelPrefab.gameObject.SetActive(true);
		labelPrefab.GetComponentInChildren<Text>().text = labelText;
		labelPrefab.position = canvas.transform.TransformPoint(pos);
	}

	public void HideLabel()
	{
		labelPrefab.gameObject.SetActive(false);
	}
	#endregion

	#region CONVERSATION UI
	public void OpenConversationPanel()
	{
		conversationPanel.SetActive(true);
	}
	
	
	public void CloseConversationPanel()
	{
		conversationPanel.SetActive(false);
	}


	public void SetOptionText(string str)
	{
		optionsTextBoxes[responseNumber].text = str;
		responseNumber++;
	}


	public void EnableResponses()
	{
		responseCanvasGroup.interactable = true;
		responseCanvasGroup.alpha = 1.0f;
	}


	public void DisableResponses()
	{
		responseCanvasGroup.interactable = false;
		responseCanvasGroup.alpha = 0.5f;
	}


	public void ClearOptions()
	{
		for(int i = 0; i < optionsTextBoxes.Length; i++)
		{
			optionsTextBoxes[i].text = null;
		}

		responseNumber = 0;
	}


	public void SetNPCTextColor(Color col)
	{
		npcConvoColor = col;
	}


	public void SetNPCDialogue(string str)
	{
		dialogueBoxText.color = npcConvoColor;
		dialogueBoxText.text = str;
	}


	public void SetPlayerDialogue(string str)
	{
		dialogueBoxText.color = playerConvoColor;
		dialogueBoxText.text = str;
	}
	

	public void ClearDialogueBoxText()
	{
		dialogueBoxText.text = null;
	}
	#endregion

	#region INVENTORY UI
	public void SelectItem(AGB_Item item, int slotId)
	{
		selectedItem = item;
		selectedItemSlotId = slotId;
	}
	
	
	public void DeselectItem()
	{
		selectedItem = null;
		selectedItemSlotId = 0;
	}
	
	
	public void DropItem()
	{
		print("Dropped item");
		
		// Remove from the slot.
		RemoveItemFromSlot(selectedItemSlotId);
		// Remove from the inventory items.
		//		TheInventory.instance.RemoveItem(selectedItem);
		// Deselect the item.
		DeselectItem();
	}
	
	
	public void StartDrag(AGB_Item item, int id)
	{
		if(!isDragging)
		{
			draggedItemStartingSlotId = id;
			draggedItem = item;
			// Instantiate the drag icon.
			dragImage = Instantiate(dragImagePrefab) as Image;
			// Set the parent.
			dragImage.transform.SetParent(itemGridRect.transform);
			// Reset the scale for some reason. 
			dragImage.transform.localScale = Vector3.one;
			// Set the anchors.
			dragImage.rectTransform.anchorMin = Vector2.zero;
			dragImage.rectTransform.anchorMax = Vector2.zero;
			// Set the pivot.
			dragImage.rectTransform.pivot = Vector2.zero;
			// Set the sprite.
			dragImage.sprite = draggedItem.iconSprite;
			// Set isDragging.
			isDragging = true;
		}
	}
	
	
	public void CancelDrag()
	{
		AddItemToSlot(draggedItemStartingSlotId, draggedItem);
		EndDrag();
	}
	
	
	public void EndDrag()
	{
		// Destroy icon under the cursor.
		Destroy(dragImage.gameObject);
		draggedItem = null;
		isDragging = false;
	}
	
	
	void GenerateSlotGrid()
	{
		int slotCount = 0;

		for(int j = 0; j < columns; j++)
		{
			// Instansiate a slot.
			Slot newSlot = Instantiate(slotPrefab) as Slot;
			// Set the slot id.
			newSlot.id = slotCount;
			// Add it to the list of slots.
			slots.Add(newSlot);
			// Set its parent.
			newSlot.transform.SetParent(itemGridRect.transform);
			// HACK Reset the scale for some reason!
			newSlot.transform.localScale = new Vector3(1, 1, 1);
			// Put it in the correct position.
			newSlot.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
			// Add the width of the slot.
			xPos = xPos + slotSize + slotPadding;

			slotCount++;
		}

/*
		for(int i = 0; i < rows; i++)
		{
			for(int j = 0; j < columns; j++)
			{
				// Instansiate a slot.
				Slot newSlot = Instantiate(slotPrefab) as Slot;
				// Set the slot id.
				newSlot.id = slotCount;
				// Add it to the list of slots.
				slots.Add(newSlot);
				// Set its parent.
				newSlot.transform.SetParent(itemGridRect.transform);
				// HACK Reset the scale for some reason!
				newSlot.transform.localScale = new Vector3(1, 1, 1);
				// Put it in the correct position.
				newSlot.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
				// Add the width of the slot.
				xPos = xPos + slotSize + slotPadding;
				// Go down a row when we get to the last one (0-based!).
				if(j == (columns - 1))
				{
					// Go to the beginning of the row (total row length).
					xPos = xPos - ((slotSize + slotPadding) * columns);
					// And down by the slot height.
					yPos = yPos - slotSize - slotPadding;
				}
				slotCount++;
			}
		}
*/		
	}
	
	
	public void AddItemToFirstEmptySlot(AGB_Item item)
	{
		//		print("Adding item to first empty slot.");
		
		// Loop through the slots.
		for(int i = 0; i < slots.Count; i++)
		{
			if(slots[i].curItem == null)
			{
				slots[i].AddItemToSlot(item);
				break;
			}
		}
	}
	
	
	public void AddItemToSlot(int i, AGB_Item item)
	{	
		// Adds an item to the slot with the correct id.
		slots[i].AddItemToSlot(item);
	}
	
	
	public void RemoveItemFromSlot(int i)
	{	
		// Adds an item to the slot with the correct id.
		slots[i].RemoveItemFromSlot();
	}
	#endregion
}
