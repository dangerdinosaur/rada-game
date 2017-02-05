using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
	public Transform target;
	public Transform endLeftTransform;
	public Transform endRightTransform;
	public float smoothTime;

	private Vector2 velocity;
	

	void Update()
	{
		if(target.position.x > endLeftTransform.position.x && target.position.x < endRightTransform.position.x)
		{
			float posX = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothTime);
			transform.position = new Vector3(posX, transform.position.y, transform.position.z);
		}
	}
}
