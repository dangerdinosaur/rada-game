using UnityEngine;
using System.Collections;

public class ut_Camera : MonoBehaviour
{
	public float panSpeed = 4.0f;
	public float panDistance = 10f;
	public float zoomSpeed = 4.0f;
	public float zoomDistance = 2.5f;


	public string stringTest;
	public int intTest;

	// TODO Set positions for near, medium, far etc. and have the functions parse out the meaning from the string.

	void OnEnable()
	{
		// Register with the Camera events.
		ut_EventManager.cameraPan += Pan;
		ut_EventManager.cameraZoom += Zoom;
	}
	
	void OnDisable()
	{
		// Remove the Camera events when destroyed.
		ut_EventManager.cameraPan -= Pan;
		ut_EventManager.cameraZoom -= Zoom;
	}


	public void Zoom(string zoomLevel)
	{
		StartCoroutine(ZoomCamera());
	}

	public void Pan(string panToPosition)
	{
		StartCoroutine(PanCamera());
	}

	IEnumerator PanCamera()
	{
		Vector3 startPos = transform.position;
		Vector3 endPos = transform.position + Vector3.right * panDistance;
		
		float i = 0.0f;
		float rate = 1.0f / panSpeed;
		
		while(i < 1.0f)
		{
			i += Time.deltaTime * rate;
			transform.position = Vector3.Lerp(startPos, endPos, i);
			yield return null;
		}
	}

	IEnumerator ZoomCamera()
	{
		float i = 0.0f;
		float rate = 1.0f / zoomSpeed;
		
		while(i < 1.0f)
		{
			i += Time.deltaTime * rate;
			GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, zoomDistance, i);
			yield return null;
		}
	}
}
