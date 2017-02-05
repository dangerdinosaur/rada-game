using UnityEngine;
using System.Collections;

public class ut_EventManager : MonoBehaviour
{
	// 1. Define delegates and events
	// 2. Define methods to call events
	// 3. Subscribe GOs to the events
	// 4. Call the events.
	
	public delegate void DialogueEvent(string arg);
	public static event DialogueEvent cameraPan;
	public static event DialogueEvent cameraZoom;
	public static event DialogueEvent playerPortrait;
	public static event DialogueEvent npcPortrait;
	public static event DialogueEvent playerAnim;
	public static event DialogueEvent npcAnim;
	public static event DialogueEvent addItem;

	
	#region Camera Events Examples
	// Argument string is the direction to pan - left, far left, right, far right.
	public static void CameraPan(string arg)
	{
		if(cameraPan != null)
		{
			cameraPan(arg);
		}
	}

	// Argument string is the zoom level - far, medium, close.
	public static void CameraZoom(string arg)
	{
		if(cameraZoom != null)
		{
			cameraZoom(arg);
		}
	}
	#endregion

	#region GUI Events Examples
	public static void AddItem(string arg)
	{
		if(addItem != null)
		{
			addItem(arg);
		}
	}
	
	public static void PlayerPortrait(string arg)
	{
		if(playerPortrait != null)
		{
			playerPortrait(arg);
		}
	}

	public static void NpcPortrait(string arg)
	{
		if(npcPortrait != null)
		{
			npcPortrait(arg);
		}
	}
	#endregion

	#region Animation Events Examples
	// Argument string is the name of the animation.
	public static void PlayerAnim(string arg)
	{
		if(playerAnim != null)
		{
			playerAnim(arg);
		}
	}

	// Argument string is the name of the animation.
	public static void NPCAnim(string arg)
	{
		if(npcAnim != null)
		{
			npcAnim(arg);
		}
	}
	#endregion
}
