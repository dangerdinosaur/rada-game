using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; private set;}

	private AudioClip dialogueAudio;

	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;

		gameObject.AddComponent<AudioSource>();
	}

	public void BeginDialogue(AudioClip passedClip)
	{
		dialogueAudio = passedClip;

		// set and play the audio clip.
		GetComponent<AudioSource>().clip = dialogueAudio;
		GetComponent<AudioSource>().Play();
	}

/*
	public void PlayDialogueAudio(string dialogueClipName)
	{
		Debug.Log("Audio request for: "+dialogueClipName);

		// Load the audioclip from the Resources folder.
		AudioClip audioClip = Resources.Load("Dialogues/"+dialogueClipName) as AudioClip;

		// set and play the audio clip.
		dialogueAudio = audioClip;
		audio.clip = dialogueAudio;
		audio.Play();
	}
*/	
}
