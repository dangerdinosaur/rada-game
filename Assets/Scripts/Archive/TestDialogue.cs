using UnityEngine;
using System.Collections;

public class TestDialogue : MonoBehaviour
{
	public string characterName;
	public string dialogClipName;


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F))
		{
			// Load the audioclip from the Resources folder.
			AudioClip audioClip = Resources.Load("Dialogues/"+characterName + "_" + dialogClipName) as AudioClip;
			// Send the clip to play through the dialog manager.
			DialogueManager.Instance.BeginDialogue(audioClip);
		}
	}
}
