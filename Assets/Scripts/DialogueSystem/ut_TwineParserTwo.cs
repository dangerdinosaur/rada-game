using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ut_TwineParserTwo : ScriptableObject
{	
	// String that will hold the source file text.
	protected string sourceFile;
	// List of all the lines in the source file.
	private List<string> sLines = new List<string>();
	
	public void OnEnable()
	{
		hideFlags = HideFlags.HideAndDontSave;
	}

	// Parse the text file.
	public Conversation.DialogueSets Parse(TextAsset twineSourceFile)
	{	
		// Load the twee source from the asset
		sourceFile = twineSourceFile.text;
		// Null the ref. to the current passage.
		DialogueSet curPassage = null;
		string curTitle = string.Empty;
		// List for the titles and the sets - so they can be serialized (Dictionaries can't!).
		List<string> titles = new List<string>();				// Temp list of titles.
		List<DialogueSet> sets = new List<DialogueSet>();		// Temp list of dialogueSets

		// Split the twee source into lines and store in an array.
		string[] lines = sourceFile.Split(new string[] {"\n"}, System.StringSplitOptions.RemoveEmptyEntries);
		// Add the array of lines to our reference var - sLines.
		sLines.AddRange(lines);

		// Loop through sLines...
		for(int i = 0; i < sLines.Count; i++)
		{	
			// This marks the start of a new passage.
			if(sLines[i].StartsWith ("::"))
			{
				// If there is already a passage - add it to passagespassages.content..
				if(curPassage != null)
				{
					titles.Add(curTitle);
					sets.Add(curPassage);
				}
				
				// Create a new passage entry.
				curPassage = new DialogueSet();
				curTitle = string.Empty;

				// TITLE
				StringBuilder sbTitle = new StringBuilder(sLines[i]);
				sbTitle.Replace("::", "");
				sbTitle.Replace("[[", "");
				sbTitle.Replace("]]", "");
				
				string[] tempStrings = sbTitle.ToString().Trim().Split(' ');
				curTitle = tempStrings[0].Trim();
				
				// EVENTS
				// If there was anything after the [, the passage has tags, so just
				// split them up and attach them to the passage.
				if(tempStrings.Length > 1)
				{
					// Loop through the temporary tags...
					for(int t = 1; t < tempStrings.Length; t++)
					{
						// Split the temp tags at ':'
						string[] linkSplit = tempStrings[t].Trim().Split(':');

						// Add the two halves of the split to 'currentPassage.dialogEvents'.
						curPassage.dialogEvents.Add(new DialogueSet.dialogueEvent { eventName = linkSplit[0], eventValue = linkSplit[1] } );
					}
				}
			}
			// Set a variable
			else if(sLines[i].StartsWith("<<set"))
			{
				// FORMAT '<<set $hasKey = "BlueKey">>' into 'hasKey BlueKey'.
				StringBuilder builder_setVar = new StringBuilder(sLines[i]);
				builder_setVar.Replace("<<set", "");
				builder_setVar.Replace("$", "");
				builder_setVar.Replace(" =", "");
				builder_setVar.Replace("\"", "");
				builder_setVar.Replace(">>", "");

				string[] setTemp = builder_setVar.ToString().Trim().Split(' ');

				// 0		    1          2 3
				// hasVisited = hasVisited + 1

				// Check if the primary condition is numeric.
				int n;
				bool isNumeric = int.TryParse(setTemp[1], out n);

				// If the primary condition is a number... 
				if(isNumeric == true)
				{
					// ...add it to the list.
					curPassage.dialogVars.Add(new DialogueSet.dialogueVariable { variableName = setTemp[0], pConditionInt = n } );
				}
				else
				{
					if(setTemp.Length <= 2)
					{
						// Simple string based variable - i.e. 'hasItem = BlueKey'
						curPassage.dialogVars.Add(new DialogueSet.dialogueVariable { variableName = setTemp[0], pConditionString = setTemp[1] } );
					}
					else
					{
						// Complex numerical variable - i.e. 'hasVisited = hasVisited + 1'
						int m;
						int.TryParse(setTemp[3], out m);
						curPassage.dialogVars.Add(new DialogueSet.dialogueVariable { variableName = setTemp[0], pConditionString = setTemp[1], sOperator = setTemp[2], sConditionInt = m } );
					}
				}
			}
			// Start an if statement - DIALOG OR RESPONSE
			else if(sLines[i].StartsWith("<<if"))
			{
				// 'IF/ELSE' STATEMENT - Complex conditional.
				if(sLines[i+2].Contains("<<else>>"))
				{
					// RESPONSE LINE
					if(sLines[i+1].StartsWith("[["))
					{
						curPassage.responseLines.Add(new DialogueSet.DialogLine {	ifStatement = ProcessIfStatement(sLines[i]),
							linkedLine_Success = ProcessLinkedLine(sLines[i+1]),
							elseStatement = true,
							linkedLine_Fail = ProcessLinkedLine(sLines[i+3])
						});
						// HACK
						sLines.RemoveRange(i, 4);
					}
					// BODY LINE
					else
					{
						curPassage.npcLines.Add(new DialogueSet.DialogLine {	ifStatement = ProcessIfStatement(sLines[i]),
							linkedLine_Success = ProcessLinkedLine(sLines[i+1]),
							elseStatement = true,
							linkedLine_Fail = ProcessLinkedLine(sLines[i+3])
						});
						// HACK
						sLines.RemoveRange(i, 4);
					}	
				}
				// 'IF' STATEMENT - Simple Conditional.
				/*
			 *  Line 1 = if statement
			 * 	Line 2 = Dialog Line - success
			 * 	Line 3 = endif DELETE
			 */
				else
				{
					if(sLines[i+1].StartsWith("[["))
					{
						// Store RESPONSE.
						curPassage.responseLines.Add(new DialogueSet.DialogLine {	ifStatement = ProcessIfStatement(sLines[i]),
							linkedLine_Success = ProcessLinkedLine(sLines[i+1]),
						});
						// HACK
						sLines.RemoveRange(i, 2);
					}
					else
					{
						// Store DIALOG.
						curPassage.responseLines.Add(new DialogueSet.DialogLine {	ifStatement = ProcessIfStatement(sLines[i]),
							linkedLine_Success = ProcessLinkedLine(sLines[i+1]),
						});
						// HACK
						sLines.RemoveRange(i, 2);
					}
				}
			}
			// PLAIN LINKED LINE
			else if(sLines[i].StartsWith("[["))
			{
				//				Debug.Log("STARTS with '[[' but no condition:" + sLines[i]);
				
				curPassage.responseLines.Add(new DialogueSet.DialogLine { linkedLine_Success = ProcessLinkedLine(sLines[i]) });
			}
			else
			{
				//				Debug.Log("A PLAIN body line with no condition:" + sLines[i]);
				curPassage.npcLines.Add(new DialogueSet.DialogLine { linkedLine_Success = ProcessLinkedLine(sLines[i]) } );
			}
		}
		
		// Catch the last passage in the twee file.
		if (curPassage != null)
		{
			titles.Add(curTitle);
			sets.Add(curPassage);
		}

		Conversation.DialogueSets dialogSets = new Conversation.DialogueSets();
		dialogSets.titles = titles;
		dialogSets.sets = sets;
		return dialogSets;
	}
	
	// Clean link lines and return an array of two strings ready to be stored.
	private DialogueSet.IfStatement ProcessIfStatement(string str)
	{
		StringBuilder sb = new StringBuilder(str);
		sb.Replace("<<if", "");
		sb.Replace(">>", "");
		sb.Replace("$", "");
		sb.Replace("\"", "");
		string[] tempStr = sb.ToString().Trim().Split(' ');
		if(tempStr.Length > 1)
		{
			DialogueSet.IfStatement newIfStatement = new DialogueSet.IfStatement { conditionA = tempStr[0], operatorString = tempStr[1], conditionB = tempStr[2] };
			return newIfStatement;
		}
		else
		{
			DialogueSet.IfStatement newIfStatement = new DialogueSet.IfStatement { conditionA = tempStr[0] };
			return newIfStatement;
		}
	}
	
	// Clean link lines and return an array of two strings ready to be stored.
	private DialogueSet.LinkedLine ProcessLinkedLine(string str)
	{
		StringBuilder sb = new StringBuilder(str);
		sb.Replace("[[", "");
		sb.Replace("]]", "");
		string[] tempStr = sb.ToString().Trim().Split('|');
		if(tempStr.Length > 1)
		{
			DialogueSet.LinkedLine nLinkedLine = new DialogueSet.LinkedLine { dialogText = tempStr[0], dialogLink = tempStr[1] };
			return nLinkedLine;
		}
		else
		{
			DialogueSet.LinkedLine nLinkedLine = new DialogueSet.LinkedLine { dialogText = tempStr[0] };
			return nLinkedLine;
		}
	}
}