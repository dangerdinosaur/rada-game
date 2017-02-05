using UnityEngine;
using System.Collections;

public class Set : MonoBehaviour
{
	public float panSpeed = 3.0f;

	public Vector2 panPosA;
	public Vector2 panPosB;
	public Vector2 panPosC;
	
	private Transform cam;


	void Awake()
	{
		cam = Camera.main.transform;
	}


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			StartPanCamera(panPosA);
		}

		if(Input.GetKeyDown(KeyCode.B))
		{
			StartPanCamera(panPosB);
		}

		if(Input.GetKeyDown(KeyCode.C))
		{
			StartPanCamera(panPosC);
		}
	}


	public void StartPanCamera(Vector2 target)
	{
		StartCoroutine(PanCamera(target));
	}


	// move the background at a slow rate - opposite to the cam.
	// move the foreground at a fast rate - opposite to the cam.
	// ScrollLayer - transform / scrollRate.

	IEnumerator PanCamera(Vector2 panToPos)
	{
		Vector3 startPos = cam.position;
		Vector3 camEndPos = new Vector3(panToPos.x, panToPos.y, -11);
		
		float i = 0.0f;
		float rate = 1.0f / panSpeed;
		
		while(i < 1.0f)
		{
			i += Time.deltaTime * rate;
			// move the camera.
			cam.position = Vector3.Lerp(startPos, camEndPos, i);
			// move the layers of the set.
			yield return null;
		}
	}
}
