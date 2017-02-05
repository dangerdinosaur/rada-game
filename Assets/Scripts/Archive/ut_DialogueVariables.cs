using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;


public class ut_DialogueVariables : MonoBehaviour
{
	[System.Serializable]
	public class dialogueVariable
	{
		public string vName;			// Name (e.g. HasVisited)
		public GameObject vTarget;		// Target gameObject (e.g. Player)
		public string vVariable;		// Variable name (e.g. hasVisited)
		public bool vBool;				// IsInt bool - if not it's a string. (e.g. true)

		public int editorHelper = 0;	// Helper value for the custom editor.
	}

	// A list of all of the dialogue variables in the scene.
	public List<dialogueVariable> variables = new List<dialogueVariable>(1);


	void AddNew()
	{
		variables.Add(new dialogueVariable());
	}

	void Remove(int index)
	{
		variables.RemoveAt(index);
	}

	// Helper bool
	public bool IsVariableInt(string varName)
	{
		// Loop through the variables.
		for(int i = 0; i < variables.Count; i++)
		{
			// If the variable name matches the passed name.
			if(variables[i].vName == varName && variables[i].vBool == true)
			{
				return true;
			}
		}
		return false;
	}
	
	// Return the variable string value.
	public string GetVariableStringValue(string varName)
	{
		// Loop through the variables.
		for(int i = 0; i < variables.Count; i++)
		{
			// If the variable name matches the passed name.
			if(variables[i].vName == varName)
			{
				GameObject go = variables[i].vTarget;
				
				// Get Components.
				MonoBehaviour[] monos = go.GetComponents<MonoBehaviour>();
				
				// Get Methods on those Components.
				foreach(MonoBehaviour mono in monos)
				{
					// Get the type.
					Type myType = mono.GetType();
					
					FieldInfo[] myField = myType.GetFields();
					for(int x = 0; x < myField.Length; x++)
					{
						if(myField[x].Name == variables[i].vVariable)
						{
							return myField[x].GetValue(mono).ToString();
						}
					}
				}
			}
		}
		print(varName + " does not exist in ut_DialogueVariables.variables - have you added it?");
		return null;
	}

	// Return the variable int value.
	public int GetVariableIntValue(string varName)
	{
		// Loop through the variables.
		for(int i = 0; i < variables.Count; i++)
		{
			// If the variable name matches the passed name.
			if(variables[i].vName == varName)
			{
				GameObject go = variables[i].vTarget;

				// Get Components.
				MonoBehaviour[] monos = go.GetComponents<MonoBehaviour>();
				
				// Get Methods on those Components.
				foreach(MonoBehaviour mono in monos)
				{
					// Get the type.
					Type myType = mono.GetType();
					
					FieldInfo[] myField = myType.GetFields();
					for(int x = 0; x < myField.Length; x++)
					{
						if(myField[x].Name == variables[i].vVariable)
						{
							return (int)myField[x].GetValue(mono);
						}
					}
				}
			}
		}
		print(varName + " does not exist in ut_DialogueVariables.variables - have you added it?");
		return 0;
	}
	
	// TODO One is update int variable value | other is update string variable value.
	void SetIntVariableValue(int varIndex, int newValue)
	{
		// Get Components.
		MonoBehaviour[] monos = variables[varIndex].vTarget.GetComponents<MonoBehaviour>();
		
		// Get Methods on those Components.
		foreach(MonoBehaviour mono in monos)
		{
			// Get the type.
			Type myType = mono.GetType();
			
			FieldInfo[] myField = myType.GetFields();
			for(int x = 0; x < myField.Length; x++)
			{
				if(myField[x].Name == variables[varIndex].vVariable)
				{
					myField[x].SetValue(mono, newValue);
				}
			}
		}
	}

	void SetStringVariableValue(int varIndex, string newValue)
	{
		// Get Components.
		MonoBehaviour[] monos = variables[varIndex].vTarget.GetComponents<MonoBehaviour>();
		
		// Get Methods on those Components.
		foreach(MonoBehaviour mono in monos)
		{
			// Get the type.
			Type myType = mono.GetType();
			
			FieldInfo[] myField = myType.GetFields();
			for(int i = 0; i < myField.Length; i++)
			{
				if(myField[i].Name == variables[varIndex].vVariable)
				{
					myField[i].SetValue(mono, newValue);
				}
			}
		}
	}
	
	// Set Variable Values - Sets both the references and the source values.
	public void SetVariableValues(DialogueSet.dialogueVariable passedVar)
	{
		// Loop through the variables.
		for(int i = 0; i < variables.Count; i++)
		{
			// If the variable name matches the passed name.
			if(variables[i].vName == passedVar.variableName)
			{
				// If the variable is an int...
				if(IsVariableInt(passedVar.variableName) == true)
				{
					// It's an Int
					// If there is an operator.
					if(passedVar.sOperator != string.Empty)
					{
						// It's a complex int.
						switch(passedVar.sOperator)
						{
						case "+":
							// Add, substract, multiply, or divide an existing variable by an int.
							SetIntVariableValue(i, GetVariableIntValue(passedVar.pConditionString) + passedVar.sConditionInt);
							break;
						case "-":
							SetIntVariableValue(i, GetVariableIntValue(passedVar.pConditionString) - passedVar.sConditionInt);
							break;
						case "*":
							SetIntVariableValue(i, GetVariableIntValue(passedVar.pConditionString) * passedVar.sConditionInt);
							break;
						case "/":
							SetIntVariableValue(i, GetVariableIntValue(passedVar.pConditionString) / passedVar.sConditionInt);
							break;
						}
					}
					else
					{
						// It's a simple int.
						SetIntVariableValue(i, passedVar.pConditionInt);
					}
				}
				else
				{
					// String
					SetStringVariableValue(i, passedVar.pConditionString);
				}
			}
		}
	}


	public bool CheckCondition(DialogueSet.IfStatement condition)
	{
		// Loop through the variables.
		for(int i = 0; i < variables.Count; i++)
		{
			if(condition.conditionA == variables[i].vName)
			{
				// If it's an int variable:
				if(variables[i].vBool == true)
				{
					int conBInt = 0;
					// If conditionB can be parsed as an int.
					if(int.TryParse(condition.conditionB, out conBInt))
					{
						// Switch on the operator.
						switch(condition.operatorString)
						{
						case "lt":
						case "<":
							if(GetVariableIntValue(condition.conditionA) < conBInt)
							{
								return true;
							}
							return false;
						case "gt":
						case ">":
							if(GetVariableIntValue(condition.conditionA) > conBInt)
							{
								return true;
							}
							return false;
						case "eq":
						case "=":
							if(GetVariableIntValue(condition.conditionA) == conBInt)
							{
								return true;
							}
							return false;
						case "neq":
						case "!=":
							if(GetVariableIntValue(condition.conditionA) != conBInt)
							{
								return true;
							}
							return false;
						case "lte":
						case "<=":
							if(GetVariableIntValue(condition.conditionA) <= conBInt)
							{
								return true;
							}
							return false;
						case "gte":
						case ">=":
							if(GetVariableIntValue(condition.conditionA) >= conBInt)
							{
								return true;
							}
							return false;
						default:
							print("Could not check the condition.");
							break;
						}
					}
				}
				// If it's a string variable:
				else
				{
					// Compare the strings.
					if(GetVariableStringValue(condition.conditionA) == condition.conditionB)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}
		return false;
	}
}
