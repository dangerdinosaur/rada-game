using UnityEngine;
using System.Collections;

public class ObjectScaler : MonoBehaviour
{	
	private Vector3 startingScale;
	private SpriteRenderer spriteRenderer;


	void Awake()
	{
		startingScale = transform.localScale;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}


	// Update is called once per frame
	void LateUpdate()
	{
		float scaleFactor = 1 - (transform.position.y / Screen.height * 100);
//		scaleFactor = 1 - scaleFactor;

		//HACK - Magic Number
		scaleFactor += 0.45f;
		transform.localScale = new Vector3(startingScale.x * scaleFactor, startingScale.y * scaleFactor, 1);
	}
}
