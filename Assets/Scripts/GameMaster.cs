using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour
{
	// Player Character
	public PolyNavAgent playerAgent;

	// Layers Masks
	public LayerMask interactableMask;
	
	// Player Movement
	private Vector3 targetPosition;
	
	// Interactables
	private bool isInteracting = false;
	private Interactable curInteractable;
	private int useWithId;

	// References
	private Camera cam;
	private AGB_Input input;

	#region REGISTER EVENTS
	void OnEnable()
	{
		AGB_Input.OnLeftClick += LeftClick;
		AGB_Input.OnLeftDoubleClick += DoubleLeftClick;
		AGB_Input.OnRightClick += RightClick;
		playerAgent.OnDestinationReached += ReachedGoal;
//		playerAgent.OnDestinationInvalid += Interact;
	}
	
	
	void OnDisable()
	{
		AGB_Input.OnLeftClick -= LeftClick;
		AGB_Input.OnLeftDoubleClick -= DoubleLeftClick;
		AGB_Input.OnRightClick -= RightClick;
		playerAgent.OnDestinationReached -= ReachedGoal;
//		playerAgent.OnDestinationInvalid -= Interact;
	}
	#endregion

	void Awake()
	{
		// Get references
		cam = Camera.main;
		input = gameObject.GetComponent<AGB_Input>();

		// Set defaults.
//		player.Speed = Utilities.walkSpeed;

		// Init the game
		input.AllowCursorMovement = true;
		input.EnableClicking = true;

		QualitySettings.vSyncCount = 0;
	}
	
	#region INPUT EVENTS
	public void LeftClick(Vector3 mPos)
	{
		Ray ray = cam.ScreenPointToRay(mPos);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, interactableMask);

		if (hit.collider != null) {
			if (hit.collider.tag == "Walkable")
			{
				isInteracting = false;
//				player.Speed = Utilities.walkSpeed;
				playerAgent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
			if(hit.collider.tag == "Interactable")
			{
				curInteractable = hit.collider.GetComponent<Interactable>();
				
				if(curInteractable != null)
				{
					isInteracting = true;
					// Move the player into position.
					targetPosition = curInteractable.interactTransform.position;
					targetPosition.z = 0;
//					player.Speed = Utilities.walkSpeed;
					playerAgent.SetDestination(targetPosition);
				}
			}
		}
		else if(hit.collider == null)
		{
			isInteracting = false;
			// PolyNav will walk to nearest spot automatically...
			playerAgent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}
	

	public void DoubleLeftClick(Vector3 mPos)
	{
		// Running..?
	}


	public void RightClick(Vector3 mPos)
	{
		// Examine..?
	}
	#endregion

	public void ReachedGoal()
	{
		// If was interacting - then do the interaction - else do nothing.
		if(isInteracting == true)
		{
			print("Interacting...");
			curInteractable.Interact();
		}
	}

/*
	#region SEQUENCES
	IEnumerator InteractSequence()
	{
		while(player.AtTarget == false)
		{
			yield return null;
		}

		curInteractable.Interact();
		
		yield return null;
	}


	IEnumerator ExamineSequence()
	{
		while(player.AtTarget == false)
		{
			yield return null;
		}

		curInteractable.Examine();
		
		yield return null;
	}

	
	public void StartUseWithSequence(Interactable target, int id)
	{
		// Set up the refs.
		curInteractable = target;
		useWithId = id;
		// Start the sequence.
		StartCoroutine("UseWithSequence");
	}


	IEnumerator UseWithSequence()
	{
		// Move player to position.
		while(player.AtTarget == false)
		{
			yield return null;
		}
		
		curInteractable.UseWith(useWithId);
		
		yield return null;
	}
	#endregion
*/	
}
