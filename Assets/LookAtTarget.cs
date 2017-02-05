using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour
{
	public Transform target;
	public float rotationDamping = 2.5f;
	private Quaternion wantedRotation;

	void Update()
	{
		wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
	}
}
