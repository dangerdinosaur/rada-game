using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Interaction
{
	public int responseId;
	public string response;
}

public class Interactable : MonoBehaviour
{
	public int interactableId;
	public string interactableName;
	public Transform interactTransform;
	public Interaction[] useWithInteractions;

	private NPC talker;

	private SpriteOutline outline;

	void OnEnable()
	{
		AGB_Input.OnHoverInteractable += Hightlight;
		AGB_Input.OnHoverNull += Unhighlight;
	}

	void OnDisable()
	{
		AGB_Input.OnHoverInteractable -= Hightlight;
		AGB_Input.OnHoverNull -= Unhighlight;
	}

	public void Awake()
	{
		talker = GetComponent<NPC>();
		outline = GetComponent<SpriteOutline>();
	}
		
	public void Hightlight(GameObject go)
	{
		if (go == this.gameObject) {
			outline.outlineSize = 1;
		}
	}


	public void Unhighlight()
	{
		outline.outlineSize = 0;
	}

	public void Interact()
	{
//		print(Utilities.RandomNegativeTalkResponse());

		TryTalk();
	}


	public void Examine()
	{
		print("Looks like a standard issue Acme rubber chicken.");
	}
	

	public bool CanUseWith(int id)
	{
		// Loop through the interactions...
		for(int i = 0; i < useWithInteractions.Length; i++)
		{
			// ...get the response matching the item id...
			if(useWithInteractions[i].responseId == id)
			{
				return true;
			}
		}
		return false;
	}


	// This is for the drag and drop inventory interactions.
	public void UseWith(int id)
	{
		// Loop through the interactions...
		for(int i = 0; i < useWithInteractions.Length; i++)
		{
			// ...get the response matching the item id...
			if(useWithInteractions[i].responseId == id)
			{
				// ...and print the relevant response.
				print(useWithInteractions[i].response);
			}
		}
	}


	public void TryTalk()
	{
		if(talker != null)
		{
			talker.TryStartConversation();
		}
		else
		{
			print(Utilities.RandomNegativeTalkResponse());
		}
	}
	

	public void RemoveFromWorld()
	{
		gameObject.SetActive (false);
	}
}
