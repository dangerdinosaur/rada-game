using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSet
{
	[System.Serializable]
	public struct dialogueEvent
	{
		public string eventName;
		public string eventValue;
	}
	[System.Serializable]
	public struct dialogueVariable
	{
		public string variableName;
		public string pConditionString;
		public int pConditionInt;
		public string sOperator;
		public int sConditionInt;
	}
	[System.Serializable]
	public struct IfStatement
	{
		public string conditionA;
		public string operatorString;
		public string conditionB;
	}
	[System.Serializable]
	public struct LinkedLine
	{
		public string dialogText;
		public string dialogLink;
	}
	[System.Serializable]
	public class DialogLine
	{
		public IfStatement ifStatement;
		public LinkedLine linkedLine_Success;
		public AudioClip audioSuccessClip;
		public bool elseStatement;
		public LinkedLine linkedLine_Fail;
		public AudioClip audioFailClip;
	}
	
	public List<dialogueEvent> dialogEvents = new List<dialogueEvent>();
	public List<dialogueVariable> dialogVars = new List<dialogueVariable>();
	public List<DialogLine> npcLines = new List<DialogLine>();
	public List<DialogLine> responseLines = new List<DialogLine>();
}
