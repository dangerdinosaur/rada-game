using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour
{
	Vector3 targetPos;


	void Update()
	{
		FaceRight();
	}


	void FaceRight()
	{
		targetPos = transform.position + Vector3.right;
		transform.LookAt(targetPos);
	}
}
