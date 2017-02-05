using UnityEngine;
using System.Collections;


public class CoroutineTest: MonoBehaviour
{
	private bool _finished = false;
	private string _button = "Jump";

	void Awake()
	{
		//This is where it all begins.
		print("waking up...");
	}

	// Use this for initialization
	void Start()
	{
		print("starting...");
		//in order to lock a coroutine down and keep it from running in parallel, we must wrap a coroutine inside of the first one.
		StartCoroutine(FirstFunction());
//		StartCoroutine(Countdown());
	}

	IEnumerator Countdown()
	{
		for(float timer = 5; timer >= 0; timer -= Time.deltaTime)
		{
			yield return null;
		}
		print("This message appears after 5 seconds.");
	}


	public IEnumerator FirstFunction()
	{
		//by default in our settings this is the space bar, so we will use this just to make it simple.
		StartCoroutine("WaitForKeyPress");
		yield return StartCoroutine("Timer");

		//this time the coroutine won't fire this print statement off until it completes
		print("Key was pressed or countdown finished.");

		yield return null;
	}
	
	public IEnumerator WaitForKeyPress()
	{
		while(!_finished)
		{
			if(Input.GetButtonDown(_button))
			{
				_finished = true;
				print("Key was pressed - skipping the dialogue.");
				StopCoroutine("Timer");
				break;
			}
			print("Awaiting key input...");
			yield return null;
		}
	}

	public IEnumerator Timer()
	{
		for(float timer = 5; timer >= 0; timer -= Time.deltaTime)
		{
			yield return null;
		}
		StopCoroutine("WaitForKeyPress");
		yield return null;
	}

	public IEnumerator WaitForTimer()
	{
		for(float timer = 5; timer >= 0; timer -= Time.deltaTime)
		{
			print("running countdown...");
			yield return null;
		}

		_finished = true;
		print("Countdown ended - going to next dialogue.");
		yield return null;
	}

	private void StartGame()
	{
		_finished = true;
		print("Starting the game officially!");
	}
}