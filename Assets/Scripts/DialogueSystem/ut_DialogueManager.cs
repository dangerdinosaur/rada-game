/// <summary>
/// ut_ DialogueManager.cs
/// 
/// Class that manages all conversations.
/// Called from ut_Talker.
/// Takes custom options from a DialogueOptions.asset file.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ut_DialogueManager : MonoBehaviour
{
	// Singleton reference.
	public static ut_DialogueManager Instance { get; private set; }

	#region Drag and Drop references.
	private UIController ui = null;			// Dialogue GUI manager.
	#endregion

	// DEV MODE
	public bool useAudio = false;

	#region Private Variables
	private Conversation convo = null;		// Current conversation file.
	private int dsKey;						// Current dialogue passage key.
	private NPC npc = null;					// Ref to the NPC
	private Player player = null;				// Ref to the Player
	#endregion

	// Options
	private float displayTime = 2.25f;
	private float timeBetweenLines = 0.25f;
	private float timeBetweenSpeakers = 1.0f;				// Gap between player and npc dialogue lines.
	private string dialogueAudioPath = "Dialogues/";			// File path in Resources that dialogue audio is stored in - i.e. 'Resources/Dialogues/' 
	private KeyCode skipKey = KeyCode.Space;


	// Awake
	void Awake()
	{
		// Singleton - does it need to be a singleton?
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;

		// Get references
		player = FindObjectOfType<Player>();
		ui = GetComponent<UIController>();
	}


	public void InitNPC(NPC newNpc)
	{
		npc = newNpc;
		convo = npc.conversation;
		ui.SetNPCTextColor(npc.textColor);
	}


	public void StartConversation()
	{
		ui.CloseInGameMenu();
		ui.OpenConversationPanel();

		// Start of every conversation is titled 'Start'.
		StartCoroutine(NpcDialogue("Start"));
	}


	public void EndConversation()
	{
		ui.ClearDialogueBoxText();
		ui.ClearOptions();
		player.IsTalking = false;
		npc.IsTalking = false;
		npc.visitCount++;
		npc = null;
	}


	// Start the npc dialogue.
	public IEnumerator NpcDialogue(string dialogueTitle)
	{
		// Check the dictionary for the correct key.
		if(convo.dialogSets.titles.Contains(dialogueTitle))
		{
			ui.DisableResponses();

			// Set the currentPassageKey to the incoming passage name.
			dsKey = convo.dialogSets.titles.IndexOf(dialogueTitle);

			// Temp strings for dialogueTitle and dialogueText.
			string tempTitle, tempText;
			
			#region Events
/*
			// Fire off any EVENTS in the DialogSet.
			if(convo.dialogSets.sets[dsKey].dialogEvents.Count > 0)
			{
				for(int j = 0; j < convo.dialogSets.sets[dsKey].dialogEvents.Count; j++)
				{
					// TODO Call by name in ut_EventManager???
					// Switch on the event name.
					switch(convo.dialogSets.sets[dsKey].dialogEvents[j].eventName)
					{
					case "CameraPan":
						ut_EventManager.CameraPan(convo.dialogSets.sets[dsKey].dialogEvents[j].eventValue);
						break;
					case "CameraZoom":
						ut_EventManager.CameraZoom(convo.dialogSets.sets[dsKey].dialogEvents[j].eventValue);
						break;
					case "PlayerPortrait":
						ut_EventManager.PlayerPortrait(convo.dialogSets.sets[dsKey].dialogEvents[j].eventValue);
						break;
					case "NpcPortrait":
						ut_EventManager.NpcPortrait(convo.dialogSets.sets[dsKey].dialogEvents[j].eventValue);
						break;
					case "PlayerAnim":
						ut_EventManager.PlayerAnim(convo.dialogSets.sets[dsKey].dialogEvents[j].eventValue);
						break;
					case "NpcAnim":
						ut_EventManager.NPCAnim(convo.dialogSets.sets[dsKey].dialogEvents[j].eventValue);
						break;
					default:
						Debug.Log("Event not recognised - check for typos in your Twine source file.");
						break;
					}
				}
			}
	*/		
			#endregion

			#region Dialogue Lines and Links
			// If the NPC has any lines of dialogue in this passage - access the dialogue line/s & play audio.
			if(convo.dialogSets.sets[dsKey].npcLines.Count > 0)
			{	
				// For each NPC line.
				for(int j = 0; j < convo.dialogSets.sets[dsKey].npcLines.Count; j++)
				{
					// If there is a conditional statement in the dialogue line...
					if(convo.dialogSets.sets[dsKey].npcLines[j].ifStatement.conditionA != string.Empty)
					{
						// Check the condition and return a bool...
						if(CheckCondition(convo.dialogSets.sets[dsKey].npcLines[j].ifStatement))
						{
							// Set the dialogueTitle and DialogueText to the 'success' variants.
							tempTitle = convo.dialogSets.sets[dsKey].npcLines[j].linkedLine_Success.dialogLink;
							tempText = convo.dialogSets.sets[dsKey].npcLines[j].linkedLine_Success.dialogText;
						}
						else
						{
							// Set the dialogueTitle and DialogueText to the 'fail' variants.
							tempTitle = convo.dialogSets.sets[dsKey].npcLines[j].linkedLine_Fail.dialogLink;
							tempText = convo.dialogSets.sets[dsKey].npcLines[j].linkedLine_Fail.dialogText;
						}
					}
					else
					{
						// If there is no conditional - set the dialogueTitle and DialogueText to the 'success' variants.
						tempTitle = convo.dialogSets.sets[dsKey].npcLines[j].linkedLine_Success.dialogLink;
						tempText = convo.dialogSets.sets[dsKey].npcLines[j].linkedLine_Success.dialogText;
					}

					// Sets the animator on ut_Talker.
					npc.IsTalking = true;

					#region GUI
					// Set 'in world' dialogue.
					ui.SetNPCDialogue(tempText);
					#endregion
					
					#region Audio
					if(useAudio)
					{
						// Load the audioclip from the Resources folder.
						// TODO User should be able to define their own path.

						AudioClip audioClip = Resources.Load("Dialogues/"+npc.characterName + "_" + tempTitle) as AudioClip;
						if(audioClip != null)
						{
							// Play the audioclip.
							npc.PlayAudioClip(audioClip);

							// Timer = length of the audio clip.
							yield return StartCoroutine(TimerSkippable(audioClip.length));
						}

						if(npc.audioSource.isPlaying == true)
						{
							npc.StopAudioClip();
						}
					}
					else
					{
						// Timer  = Default line time delay.
						yield return StartCoroutine(TimerSkippable(displayTime));
					}
					#endregion

					// Animator to stop talking animation.
					npc.IsTalking = false;

					// for all lines but the last...
					if((j + 1) < convo.dialogSets.sets[dsKey].npcLines.Count)
					{
						// Optional delay - time between lines.
						yield return new WaitForSeconds(timeBetweenLines);
					}				
				}
			}
			#endregion
			
			// If it's the end of the the convo...
			if(dialogueTitle == "End")
			{
				//...end the conversation.
				EndConversation();
				// UI - close dialogue panel.
				ui.CloseConversationPanel();
				// UI - open in game panel.
				ui.OpenInGameMenu();
			}
			else
			{
				ui.ClearOptions();
				// Else - Init the player's responses.
				SetPlayerResponses();
			}
			yield return null;
		}
		else
		{
			yield return null;
		}
	}

	// Start the player dialogue.
	public IEnumerator PlayerDialogue(int optionNumber)
	{
		// Access dialogue line / play audio clip / with skippable timer.
		if(convo.dialogSets.sets[dsKey].responseLines.Count > 0)
		{
			ui.DisableResponses();

			// Get the name of the dialogClip
			string tempTitle = convo.dialogSets.sets[dsKey].responseLines[optionNumber].linkedLine_Success.dialogLink;
			string tempText = convo.dialogSets.sets[dsKey].responseLines[optionNumber].linkedLine_Success.dialogText;

			// If the developer wants to use dialogue audio...
			if(useAudio)
			{
				// Load the audioclip from the Resources folder specified in the DialogueOptions file. Default is 'Resources/Dialogues/'.
				// TODO - Make this so it works with multiple 'players' - Maniac Mansion situation??
				AudioClip audioClip = Resources.Load(dialogueAudioPath + "Player_" + tempTitle) as AudioClip;
				if(audioClip != null)
				{
					// Send the clip to play through the dialog manager.
					player.PlayAudioClip(audioClip);

					// Display the dialogue.
					ui.SetPlayerDialogue(tempText);
					
					// Wait until the timer has gone down or the player has skipped.
					yield return StartCoroutine(TimerSkippable(audioClip.length));
				}

				if(npc.audioSource.isPlaying == true)
				{
					npc.StopAudioClip();
				}
			}

			// If the developer doesn't want to use dialogue audio.
			else if(!useAudio)
			{
				// Display the subtitle.
				ui.SetPlayerDialogue(tempText);

				// Wait until the timer has gone down or the player has skipped.
				yield return StartCoroutine(TimerSkippable(displayTime));
			}

			// Optional delay - time between speakers.
			yield return new WaitForSeconds(timeBetweenSpeakers);

			// Call the next NPC Dialog.
			StartCoroutine(NpcDialogue(tempTitle));

			yield return null;
		}
		yield return null;
	}

	#region Player Responses
	public void SetPlayerResponses()
	{
		ui.EnableResponses();

		// Response Lines.
		if(convo.dialogSets.sets[dsKey].responseLines.Count > 0)
		{
			// For each option.
			for(int i = 0; i < convo.dialogSets.sets[dsKey].responseLines.Count; i++)
			{
				// If there is an IfStatement in the line.
				if(convo.dialogSets.sets[dsKey].responseLines[i].ifStatement.conditionA != string.Empty)
				{
					// Check the condition
					if(CheckCondition(convo.dialogSets.sets[dsKey].responseLines[i].ifStatement))
					{
						// Send the info to the GUI manager.
						ui.SetOptionText(convo.dialogSets.sets[dsKey].responseLines[i].linkedLine_Success.dialogText);	                      
					}
					else
					{
						// If there is a 'Failed Check' state... 
						if(convo.dialogSets.sets[dsKey].responseLines[i].elseStatement == true)
						{
							// Create the 'Failed Check' response.
							ui.SetOptionText(convo.dialogSets.sets[dsKey].responseLines[i].linkedLine_Fail.dialogText);	                      
						}
					}
				}
				else
				{
					// If there is no IfStatement - just create the success response.
					ui.SetOptionText(convo.dialogSets.sets[dsKey].responseLines[i].linkedLine_Success.dialogText);			                      
				}
			}
		}
	}
	#endregion

	// A timer that can be skipped with a button press. The button is defined in DialogueOptions.
	public IEnumerator TimerSkippable(float secondsLeft)
	{
		while(secondsLeft > 0f)
		{
			secondsLeft -= Time.deltaTime;
			
			if(Input.GetKeyDown(skipKey))
			{
				if(useAudio && GetComponent<AudioSource>().isPlaying == true)
				{
					GetComponent<AudioSource>().Stop();
				}

				yield return null;

				break;
			}
			yield return null;
		}
	}


	// Called when player clicks on a dialog option.
	public void SelectOption(int optionNumber)
	{
		StartCoroutine(PlayerDialogue(optionNumber));
	}


	public void SelectOptionA()
	{
		SelectOption(0);
	}


	public void SelectOptionB()
	{
		SelectOption(1);
	}


	public void SelectOptionC()
	{
		SelectOption(2);
	}


	public bool CheckCondition(DialogueSet.IfStatement condition)
	{
		switch(condition.conditionA)
		{
		case "hasItem":
			if(player.HasItem(condition.conditionB))
			{
				return true;
			}
			return false;
		case "hasVisited":
			if(player.HasVisited(condition.conditionB))
			{
				return true;
			}
			return false;
		case "visitCount":
			int conBInt = 0;

			// If conditionB can be parsed as an int.
			if(int.TryParse(condition.conditionB, out conBInt))
			{
				// Switch on the operator.
				switch(condition.operatorString)
				{
				case "lt":
				case "<":
					if(npc.visitCount < conBInt)
					{
						return true;
					}
					return false;
				case "gt":
				case ">":
					if(npc.visitCount > conBInt)
					{
						return true;
					}
					return false;
				case "eq":
				case "=":
					if(npc.visitCount == conBInt)
					{
						return true;
					}
					return false;
				case "neq":
				case "!=":
					if(npc.visitCount != conBInt)
					{
						return true;
					}
					return false;
				case "lte":
				case "<=":
					if(npc.visitCount <= conBInt)
					{
						return true;
					}
					return false;
				case "gte":
				case ">=":
					if(npc.visitCount >= conBInt)
					{
						return true;
					}
					return false;
				default:
					print("Could not check the condition.");
					break;
				}
			}
			return false;
		default:
			print("Could not check the condition.");
			break;
		}
		return false;
	}
}
