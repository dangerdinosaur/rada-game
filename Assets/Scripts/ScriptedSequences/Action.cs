using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour
{
	private bool _isComplete = false;
	public bool IsComplete
	{
		get { return _isComplete; }
		set { _isComplete = value; }
	}
	
	public float preDelay;
	public float postDelay;


	public virtual void Execute()
	{
		// Do something.
	}
}
