using UnityEngine;
using System.Collections;

public class Unit3D : MonoBehaviour
{
	public Transform target;
	private NavMeshAgent _agent;


	// Use this for initialization
	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
	}


	// Update is called once per frame
	void Update()
	{
		
	}
}
