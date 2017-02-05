using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour
{
	public string characterName;
	public Conversation conversation;
	public Color textColor;
	public Sprite charPortrait;
	
	[HideInInspector]
	public int visitCount;
	
	private Animator animator;
	[HideInInspector]
	public AudioSource audioSource;
	private bool _isTalking = false;
	
	
	void OnEnable()
	{
		ut_EventManager.npcAnim += PlayAnimation;
	}
	
	
	void OnDisable()
	{
		ut_EventManager.npcAnim -= PlayAnimation;
	}
	
	
	void Awake()
	{
		// Get reference to the Animator component.
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}
	
	
	// Call the dialogue manager with the conversation reference.
	public void TryStartConversation()
	{
		// Register the npc character with the DialogueManager.
		ut_DialogueManager.Instance.InitNPC(this);
		// Start conversation.
		ut_DialogueManager.Instance.StartConversation();
	}


	public void PlayAudioClip(AudioClip clip)
	{
		audioSource.clip = clip;
		audioSource.Play();
	}


	public void StopAudioClip()
	{
		audioSource.Stop();
	}

	
	public bool IsTalking
	{
		get { return _isTalking; }
		set
		{
			_isTalking = value;
			
			if(animator != null)
			{
				if(_isTalking)
				{
					animator.SetBool("isTalking", true);
				}
				else if(!_isTalking)
				{
					animator.SetBool("isTalking", false);
				}
			}
		}
	}
	
	
	public void PlayAnimation(string animationName)
	{
		animator.SetTrigger(animationName);
	}
}
