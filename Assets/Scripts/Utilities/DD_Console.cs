using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

// DD_Console Class
public class DD_Console : MonoBehaviour
{
	// Singleton pattern...
	private static DD_Console _instance;
	public static DD_Console Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = GameObject.FindObjectOfType(typeof(DD_Console)) as DD_Console;
				if (!_instance)
				{
					GameObject container = new GameObject();
					container.name = "MyClassContainer";
					_instance = container.AddComponent(typeof(DD_Console)) as DD_Console;
				}
			}
			return _instance;
		}
	}

	public InputField inputField;

	public GameObject consoleGO;
	private Text consoleText;
	bool isOpen = false;

	private int _lineNumber = 0;
	private int _prevMultiplier = 1;
	private string _prevLine;
	private List<string> _loggedLines = new List<string>();
	private string _completeLog;


	void Start()
	{
		consoleText = consoleGO.GetComponentInChildren<Text>();
		consoleGO.SetActive(false);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			ToggleConsole();
		}
/*
		if(Input.GetKeyDown(KeyCode.T))
		{
			Log("Player did something interesting at timecode: " + Time.time);
			if(isOpen)
			{
				DisplayLog();
			}
		}
*/	
	}
	
	void ToggleConsole()
	{
		if(!isOpen)
		{
			consoleGO.SetActive(true);
			isOpen = true;
			DisplayLog();
			inputField.ActivateInputField();
		}
		else
		{
			inputField.DeactivateInputField();
			consoleGO.SetActive(false);
			isOpen = false;
		}
	}

	public void TryToParse()
	{
		if(inputField.text == "")
			return;

//		string str = inputField.text;
		string[] tempStrings = inputField.text.Trim().Split(' ');
		string command = tempStrings[0];

		switch(command)
		{
		case "sv_giveallitems":
			int val;
			bool isInt = int.TryParse(tempStrings[2], out val);
			if(isInt)
			{
				Run(tempStrings[0], val);
			}
			break;
		case "listall":
			Log("sv_giveallitems | log | listall");
			DisplayLog();
			break;
		case "log":
			StringBuilder sb = new StringBuilder(inputField.text);
			sb.Replace("log ", "");
			Log(sb.ToString());
			DisplayLog();
			break;
		default:
			Log("Command not recognised. Type 'listall' for a list of available commands.");
			DisplayLog();
			break;
		}
/*
		if(str.StartsWith("sv_giveallitems"))
		{
			string[] tempStrings = str.Trim().Split(' ');
			int val;
			bool isInt = int.TryParse(tempStrings[2], out val);
			if(isInt)
			{
				Run(tempStrings[0], val);
			}
		}
		else if(str.StartsWith("log"))
		{
			StringBuilder sb = new StringBuilder(str);
			sb.Replace("log ", "");
			Log(sb.ToString());
			DisplayLog();
		}
		else
		{
			Log("Command not recognised. Type 'listall' for a list of available commands.");
			DisplayLog();
		}
*/
		// Clear the input field.
		inputField.text = "";
		// Activate the input field.
		inputField.ActivateInputField();
	}

	// Call a method with an int arg.
	public void Run(string methodName, int intValue)
	{
		// Does the command exist?
		// Run the command.
		// Print a message to the console.

		Type t = this.GetType();
		MethodInfo method = t.GetMethod(methodName);
		method.Invoke(this, new object[] { intValue });
	}

	// Call a method with a string arg.
	public void Run(string methodName, string strValue)
	{
		// Does the command exist?
		// Run the command.
		// Print a message to the console.
		
		Type t = this.GetType();
		MethodInfo method = t.GetMethod(methodName);
		method.Invoke(this, new object[] { strValue });
	}

	// e.g. 'Godmode = 1'
	public void IntValues(int value)
	{
		Log("Testing ints = " + value);
	}

	// e.g. 'Give player = rocket'
	public void StringValues(string value)
	{
		Log("Testing strings = " + value);
	}

	public void sv_giveallitems(int value)
	{
		Log("sv_giveallitems = " + value + " Woo!");
		DisplayLog();
		// call cheat command to actuall give all items.
	}

	// Print the logged lines.
	public void PrintLog()
	{
		for(int i = 0; i < _loggedLines.Count; i++)
		{
			_completeLog += _loggedLines[i] + "\n";
		}

		print(_completeLog);
	}

	public void DisplayLog()
	{
		// Clear the text.
		string _log = null;
		// Start with an empty line.
		_log += "\n";
		// Loop through the log.
		for(int i = 0; i < _loggedLines.Count; i++)
		{
			_log += _loggedLines[i] + "\n";
		}
		// Display to the text element.
		consoleText.text = _log;
	}

	
	/* How to change a line.
	StringBuilder builder = new StringBuilder(
	"This is an example string that is an example.");
	builder.Replace("an", "the"); // Replaces 'an' with 'the'.
	Console.WriteLine(builder.ToString());
	Console.ReadLine();
	 */

	// Log a string to our console.
	public void Log(params string[] lines)
	{
		if(lines == null)
		{
			// Warn the user that str is null.
			print("nothing to log...");
		}
		else
		{
			for(int i = 0; i < lines.Length; i++)
			{
				// If this line is the same as the last line...
				if(lines[i] == _prevLine)
				{
					// Set the multiplier.
					_prevMultiplier++;
					// Remove the last logged line.
					_loggedLines.RemoveAt(_lineNumber - 1);
					// Add the logged line (with multiplier)
					_loggedLines.Add(_lineNumber + ". " + lines[i] + " [x" + _prevMultiplier + "]");
				}
				else if(lines[i] != _prevLine)
				{
					// Increment the lineNumber value.
					_lineNumber++;
					// Reset the multiplier 
					_prevMultiplier = 1;
					// Set prev line.
					_prevLine = lines[i];
					// Add the logged line (clean).
					_loggedLines.Add(_lineNumber + ". " + lines[i]);
				}
			}
		}
	}

}
