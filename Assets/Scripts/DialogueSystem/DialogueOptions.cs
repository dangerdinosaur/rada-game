using UnityEngine;
using System.Collections;


[System.Serializable]
public class DialogueOptions : ScriptableObject
{
	#region Input
	public KeyCode skipKey = KeyCode.Space;
//	public KeyCode[] skipKeys;								// TODO an array of skip keys... Add to them? then loop through?
	#endregion

	#region Audio Options
	public bool useAudio = false;							// Use audio or not.
	public float timeBetweenLines = 2.25f;					// The default time delay between a character's dialogue lines - only used if they have multiple lines.
	public float timeBetweenSpeakers = 1.0f;				// Gap between player and npc dialogue lines.
	public string dialogueAudioPath = "Dialogues/";			// File path in Resources that dialogue audio is stored in - i.e. 'Resources/Dialogues/' 
	#endregion

	#region OnScreenDialogue
	public enum DisplayDialogue { none, all, player, npc };
	
	public DisplayDialogue displayDialogue;

	public enum ClearDialogue { none, all, player, npc };
	public ClearDialogue clearDialogue;

	public float displayTime;
	#endregion

	#region Responses
	public enum ResponsePrefix { none, prefixNumber, prefixLetter };
	public ResponsePrefix responsePrefix;

	public bool response_HighlightFirst;
	
	public enum ResponseBehaviourOnSpeak { disable, clear };
	public ResponseBehaviourOnSpeak responseBehaviour;
	#endregion

	#region Subtitles
	public enum Subtitles { none, all, player, npc };
	public Subtitles subtitles;
	
	public enum SubtitlePrefix { none, names };
	public SubtitlePrefix subtitlePrefix;
	
	public Color subtitleColor;
	public Font subtitleFont;

	
	public bool test1;
	public bool test2;
	#endregion
}
