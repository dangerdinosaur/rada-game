using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	// Player Character
	public Transform player;
	
	// Click Masks
	public LayerMask walkableMask;
	public LayerMask interactableMask;

	// Speed and Distance - move to player script.
	private float moveSpeed = 3.0f;
	private float minMoveDistance = 0.1f;

	// Positioning
	private Vector3 targetPosition;
	private float targetDistance;

	// Camera Reference
	private Camera cam;

	private bool playerIsMoving;


	void Awake()
	{
		// Init
		cam = Camera.main;
		targetPosition = player.position;
	}


	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, interactableMask);

			if(hit.collider == null)
				return;

			IInteractable interactable = hit.collider.GetComponent<IInteractable>();

			if(interactable == null)
				return;

			interactable.Interact();
		}
	}
	

	public void TryToConverse(Interactable interactable)
	{
		StartCoroutine("TalkSequence");
	}
	
	
	IEnumerator TalkSequence(Interactable targetObject)
	{
		targetPosition = targetObject.interactTransform.position;
		targetPosition.z = 0;
		
		//If player is already moving - stop them.
		if(playerIsMoving)
		{
			StopCoroutine("MovePlayer");
		}

		// Start movement coroutine / wait for player to reach destination.
		yield return StartCoroutine("MovePlayer");
		
		targetObject.TryTalk();
		
		yield return null;
	}


	public void TryToMovePlayer(Vector3 newTarget)
	{
		targetPosition = newTarget;
		targetPosition.z = 0;
		
		// If player is already moving - stop them.
		if(playerIsMoving)
		{
			StopCoroutine("MovePlayer");
		}
		// Start movement coroutine.
		StartCoroutine("MovePlayer");
	}

	
	IEnumerator MovePlayer()
	{
		playerIsMoving = true;
		
		// While the distance to target is greater than the minimum move distance...
		while(Vector3.Distance(targetPosition, player.position) > minMoveDistance)
		{
			// Move the player character towards the target position.
			//			Vector3 roundPos = new Vector3 (RoundToNearestPixel (targetPosition.x), RoundToNearestPixel (targetPosition.y), targetPosition.z);
			player.position = Vector3.MoveTowards(player.position, targetPosition, moveSpeed * Time.deltaTime);
			
			yield return null;
		}
		
		playerIsMoving = false;
		// print("Reached destination.");
	}


	private float RoundToNearestPixel(float units)
	{
		float valueInPixels = (Screen.height / (cam.orthographicSize * 2)) * units;
		valueInPixels = Mathf.Round(valueInPixels);
		float adjustedUnits = valueInPixels / (Screen.height / (cam.orthographicSize * 2));
		return adjustedUnits;
	}

}
