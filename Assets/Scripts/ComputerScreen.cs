using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


public class ComputerScreen : MonoBehaviour
{
	public Text screenTextBox = null;
	public InputField inputField = null;

	private int _lineNumber = 0;
	private int _prevMultiplier = 1;
	private string _prevLine;
	private List<string> _loggedLines = new List<string>();
	private string _completeLog;


	void Start()
	{
		inputField.Select();
	}


	public void ParseInput()
	{
		if(inputField.text == null)
			return;

		string[] tempStrings = inputField.text.Trim().Split(' ');
		string command = tempStrings[0];

		// COMMANDS
		switch(command)
		{
		case "sv_giveallitems":
			if(tempStrings.Length < 2)
				return;

			int val;
			bool isInt = int.TryParse(tempStrings[2], out val);
			if(isInt)
			{
				Run(tempStrings[0], val);
			}
			break;
		case "fillscreen":
			Run(tempStrings[0], 0);
			break;
		case "listall":
			LogLine("sv_giveallitems | log | listall");
			DisplayLines();
			break;
		case "log":
			StringBuilder sb = new StringBuilder(inputField.text);
			sb.Replace("log ", "");
			LogLine(sb.ToString());
			DisplayLines();
			break;
		default:
			LogLine("Command not recognised... Type 'listall' for a list of available commands.");
			DisplayLines();
			break;
		}

		// Clear the input field.
		inputField.text = "";
		// Activate the input field.
		inputField.ActivateInputField();
	}


	public void DisplayLines()
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
		screenTextBox.text = _log;
	}


	// Log a string to our console.
	public void LogLine(params string[] lines)
	{
		if(lines == null)
			return;

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
		LogLine("Testing ints = " + value);
	}


	// e.g. 'Give player = rocket'
	public void StringValues(string value)
	{
		LogLine("Testing strings = " + value);
	}

	#region COMMAND METHODS
	public void sv_giveallitems(int value)
	{
		if(value == 0)
		{
			LogLine("sv_giveallitems = 0 | You now have no items :(");
		}
		else if(value == 1)
		{
			LogLine("sv_giveallitems = 1 | You now have all items :)");
		}
		else
		{
			LogLine("sv_giveallitems is a boolean value - its value can only be 0 or 1");
		}

		DisplayLines();
	}

	public void fillscreen(int value)
	{
		for(int i = 0; i < 25; i++)
		{
			LogLine("Filling the screen with lines... " + i);
		}

		DisplayLines();
	}
	#endregion
}
