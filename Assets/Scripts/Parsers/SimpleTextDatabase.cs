using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SimpleTextDatabase : ScriptableObject
{
	public TextAsset sourceFile;
	public string lastUpdated;		// A record of the last time the sourceFile was parsed into titles and sets.
	public List<SimpleTextLine> lines = new List<SimpleTextLine>();


	public void UpdateDatabase()
	{
		// Create an instance of the parse ScriptableObject.
		SimpleTextParser parser = ScriptableObject.CreateInstance("SimpleTextParser") as SimpleTextParser;
		// Then parse the conversation source file.
		lines = parser.Parse(sourceFile);
		lastUpdated = DateAndTimeCreated();
	}


	// Method to return the date and time created.
	public string DateAndTimeCreated()
	{
		string dateTime = System.DateTime.Now.ToString("hh:mm:ss") + " | " + System.DateTime.Now.ToString("MM/dd/yyyy");
		return dateTime;
	}
}
