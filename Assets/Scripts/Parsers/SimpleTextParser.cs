using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SimpleTextParser : ScriptableObject
{
	// String that will hold the source file text.
	protected string sourceFile;
	// List of all the lines in the source file.
	private List<string> sourceLines = new List<string>();


	public void OnEnable()
	{
		hideFlags = HideFlags.HideAndDontSave;
	}


	public List<SimpleTextLine> Parse(TextAsset textSourceFile)
	{
		// Load the twee source from the asset
		sourceFile = textSourceFile.text;
		// Declare a new set of lines.
		List<SimpleTextLine> updatedLines = new List<SimpleTextLine>();

		// Split the twee source into lines and store in an array.
		string[] lines = sourceFile.Split(new string[] {"\n"}, System.StringSplitOptions.RemoveEmptyEntries);
		// Add the array of lines to our reference var - sLines.
		sourceLines.AddRange(lines);

		// Loop through sLines...
		for(int i = 0; i < sourceLines.Count; i++)
		{
			if(sourceLines[i].StartsWith("//"))
				continue;

			// Declare a new line with the value of i and the string.
			SimpleTextLine newLine = new SimpleTextLine { id = i, lineText = sourceLines[i] };
			// Add that to the database.
			updatedLines.Add(newLine);
		}

		// Return the list of lines.
		return updatedLines;
	}
}
