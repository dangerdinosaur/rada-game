using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public Sprite charPortrait;

	private bool _isTalking = false;
	private Animator animator;
	[HideInInspector]
	public AudioSource audioSource;
	
	
	void OnEnable()
	{
		ut_EventManager.playerAnim += PlayAnimation;
	}
	
	
	void OnDisable()
	{
		ut_EventManager.playerAnim -= PlayAnimation;
	}
	
	
	void Awake()
	{
		// Get reference to the Animator component.
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
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


	public bool HasItem(string str)
	{
		return true;
	}


	public bool HasVisited(string str)
	{
		return true;
	}


	public void PlayAnimation(string animationName)
	{
		animator.SetTrigger(animationName);
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
}
