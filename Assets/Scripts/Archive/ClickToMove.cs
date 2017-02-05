using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace DAScripts {

public class ClickToMove : MonoBehaviour
{
	// Click Mask
//	public LayerMask moveMask;

	// Speed and Distance
//	private float moveSpeed;
//	public float walkSpeed = 0.5f;
//	private float minMoveDistance = 0.05f;

	// Positioning
	private Vector3 startingPosition;
	private Vector3 targetPosition;
	private float targetDistance;
	private Vector3 moveDirection;
	private bool facingRight = true;

	// Animation
	private Animator anim;

	// Camera Reference
	private Camera cam;

	public bool doFlip = true;

	private Vector2 lastDir;
	private float originalScaleX;

	private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent){
				_agent = GetComponent<PolyNavAgent>();
			}
			return _agent;			
		}
	}

	void Awake(){
		originalScaleX = transform.localScale.x;
	}

	void Start()
	{
		targetPosition = transform.position;
		cam = Camera.main;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update()
	{
		// WALKING
		if (agent.currentSpeed > 0.1f)
		{
			anim.SetBool ("isWalking", true);
		}
		else if(agent.currentSpeed <= 0.1f)
		{
			anim.SetBool ("isWalking", false);
		}
		
		// DIRECTION
		var dir = agent.movingDirection;
		var x = Mathf.Round(dir.x);
		var y = Mathf.Round(dir.y);

		//eliminate diagonals favoring x over y
		y = Mathf.Abs(y) == Mathf.Abs(x)? 0 : y;

		dir = new Vector2(x, y);

		if (dir != lastDir){

			if (dir == Vector2.zero){
				Debug.Log("IDLE");
			}

			if (dir.x == 1){
				Debug.Log("RIGHT");
				if (doFlip){
					var scale = transform.localScale;
					scale.x = originalScaleX;
					transform.localScale = scale;
				}
			}

			if (dir.x == -1){
				Debug.Log("LEFT");
				if (doFlip){
					var scale = transform.localScale;
					scale.x = -originalScaleX;
					transform.localScale = scale;
				}
			}

			if (dir.y == 1){
				Debug.Log("UP");
			}

			if (dir.y == -1){
				Debug.Log("DOWN");
			}

			lastDir = dir;
		}
/*		
		if(Input.GetKey(KeyCode.Mouse0))
		{
			if(EventSystem.current.IsPointerOverGameObject())
				return;

			agent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
*/
	}
	
/*
	private float RoundToNearestPixel(float units)
	{
		float valueInPixels = (Screen.height / (cam.orthographicSize * 2)) * units;
		valueInPixels = Mathf.Round(valueInPixels);
		float adjustedUnits = valueInPixels / (Screen.height / (cam.orthographicSize * 2));
		return adjustedUnits;
	}
*/
/*
	// Flip the sprite left and right
	void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;

		theScale.x *= -1;
		transform.localScale = theScale;
	}
	*/
}
}
