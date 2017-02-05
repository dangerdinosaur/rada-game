using UnityEngine;
using System.Collections.Generic;

// Should be serializable!?
[System.Serializable]
public class Conversation : ScriptableObject
{
	// TODO Put this somewhere else? With all the other constructors?
	[System.Serializable]
	public class DialogueSets 
	{
		public List<string> titles;		// A list of dialogueSet titles.
		public List<DialogueSet> sets;	// A list of dialogueSets.
	}

	public TextAsset sourceFile;
	public bool autoUpdate;			// A bool to set whether to parse the sourceFile on Awake or not.
	public string lastUpdated;		// A record of the last time the sourceFile was parsed into titles and sets.
	public DialogueSets dialogSets;


	public void Awake()
	{
		if(autoUpdate)
		{
			// Parse the file.
			UpdateConversation();
		}
	}


	public void UpdateConversation()
	{
		// Create an instance of the parse ScriptableObject.
		ut_TwineParserTwo parser = ScriptableObject.CreateInstance("ut_TwineParserTwo") as ut_TwineParserTwo;
		// Then parse the conversation source file.
		dialogSets = parser.Parse(sourceFile);
		lastUpdated = DateAndTimeCreated();
	}

	// Method to return the date and time created.
	public string DateAndTimeCreated()
	{
		string dateTime = System.DateTime.Now.ToString("hh:mm:ss") + " | " + System.DateTime.Now.ToString("MM/dd/yyyy");
		return dateTime;
	}
}
