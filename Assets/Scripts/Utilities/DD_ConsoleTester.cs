using UnityEngine;
using System.Collections;

public class DD_ConsoleTester : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		DD_Console.Instance.Log("This is a test message");
		DD_Console.Instance.Run("IntValues", 8);
		DD_Console.Instance.Run("StringValues", "This is a pickle.");
		DD_Console.Instance.Log("This is a debug message", "This is another debug message");
		DD_Console.Instance.Log("This is a debug message");
		DD_Console.Instance.Log("This is a debug message");
		DD_Console.Instance.Log("Player was shot in the leg");
		DD_Console.Instance.Log("Player died!");
	}
}
