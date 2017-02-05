using UnityEngine;
using System.Collections;

public class ActionSequence : MonoBehaviour
{
	private Action[] actions;
	private bool isPlaying = false;


	void Awake()
	{
		actions = gameObject.GetComponents<Action>();
	}


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.T))
		{
			Play();
		}
	}


	public void Play()
	{
		if(isPlaying)
			return;

		StartCoroutine(PlaySequence());
	}


	IEnumerator PlaySequence()
	{
		isPlaying = true;

		int i = 0;

		while(true)
		{
			// Execute the  action.
			if(i < actions.Length)
			{
				actions[i].Execute();

				// Wait for the action to complete.
				while(!actions[i].IsComplete)
				{
					yield return null;
				}
			}
			else
			{
				yield return null;
			}

			i++;

			yield return null;
		}
	}
}
