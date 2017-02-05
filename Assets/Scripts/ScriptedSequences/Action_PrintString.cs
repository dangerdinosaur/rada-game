using UnityEngine;
using System.Collections;

public class Action_PrintString : Action
{
	public string testMessage;

	public override void Execute()
	{
		StartCoroutine(PrintString());
	}


	IEnumerator PrintString()
	{
//		IsComplete = false;

		print("...");

		yield return new WaitForSeconds(preDelay);

		print(testMessage);

		yield return new WaitForSeconds(postDelay);

		IsComplete = true;

		yield return null;
	}
}
