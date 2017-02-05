using UnityEngine;
using System.Collections;

public class AgentController : MonoBehaviour
{
	private Animator anim;
	private NavMeshAgent navMeshAgent;
	private Ray shootRay;
	private bool walking;


	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}


	// Update is called once per frame
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Input.GetMouseButtonDown(0)) 
		{
			if (Physics.Raycast(ray, out hit, 100))
			{
				walking = true;
//				enemyClicked = false;
				navMeshAgent.destination = hit.point;
				navMeshAgent.Resume();
			}
		}
		
		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			if(!navMeshAgent.hasPath || Mathf.Abs (navMeshAgent.velocity.sqrMagnitude) < float.Epsilon)
				walking = false;
		}
		else
		{
			walking = true;
		}
	}
}
