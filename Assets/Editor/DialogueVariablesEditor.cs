using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;


[CustomEditor(typeof(ut_DialogueVariables))]
public class DialogueVariablesEditor : Editor
{
	private static GUIContent
		addNewButtonContent = new GUIContent("+ New Variable", "Add a new dialogue variable"),
		addButtonContent = new GUIContent("+", "add new variable"),
		deleteButtonContent = new GUIContent("-", "delete this variable");
	
	private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
	private static GUILayoutOption bigButtonWidth = GUILayout.Width(100f);
	
	private List<string> fields = new List<string>();
	private List<FieldInfo> fieldInfos = new List<FieldInfo>();
	private ut_DialogueVariables t;
	private SerializedObject GetTarget;
	private SerializedProperty ThisList;


	// Set up references
	void OnEnable()
	{
		t = (ut_DialogueVariables)target;					// Reference to the target script.
		GetTarget = new SerializedObject(t);				// Call the target script.
		ThisList = GetTarget.FindProperty("variables");		// Find the List in our script and create a refrence of it
	}

	// GUI
	public override void OnInspectorGUI()
	{
		//Update our list
		GetTarget.Update();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		// Title of the variable toolbar
		GUILayout.Label("Dialogue Variables Editor", EditorStyles.toolbarButton);

		// Add new variable button.
		if(GUILayout.Button(addNewButtonContent, EditorStyles.toolbarButton, bigButtonWidth))
		{
			t.variables.Add(new ut_DialogueVariables.dialogueVariable());
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		//Display our list to the inspector window
		for(int i = 0; i < ThisList.arraySize; i++)
		{
			SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);

			if(ThisList.GetArrayElementAtIndex(i) != null)
			{
				SerializedProperty vName = MyListRef.FindPropertyRelative("vName");
				SerializedProperty vTarget = MyListRef.FindPropertyRelative("vTarget");
				SerializedProperty vVariable = MyListRef.FindPropertyRelative("vVariable");
				SerializedProperty vBool = MyListRef.FindPropertyRelative("vBool");
				SerializedProperty helperIndex = MyListRef.FindPropertyRelative("editorHelper");

				EditorGUILayout.Space();

				// NEW!
				GUILayout.BeginVertical(EditorStyles.textArea);

				EditorGUILayout.BeginHorizontal();
				// Title of the variable toolbar
				if(vName.stringValue != string.Empty)
				{
					GUILayout.Label(vName.stringValue, EditorStyles.toolbarButton);
				}
				else
				{
					GUILayout.Label("Unnamed Variable " + i, EditorStyles.toolbarButton);
				}

				// Add new variable button.
				if(GUILayout.Button(addButtonContent, EditorStyles.toolbarButton, miniButtonWidth))
				{
					t.variables.Add(new ut_DialogueVariables.dialogueVariable());
				}
				// Delete this variable button. 
				if(GUILayout.Button(deleteButtonContent, EditorStyles.toolbarButton, miniButtonWidth))
				{
					int oldSize = ThisList.arraySize;
					ThisList.DeleteArrayElementAtIndex(i);
					if(ThisList.arraySize == oldSize)
					{
						ThisList.DeleteArrayElementAtIndex(i);
					}
				}
				// If you click delete - all of the references below will throw errors without this else.
				else
				{
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					// Unique variable name field.

					EditorGUILayout.LabelField("Unique Identifier", GUILayout.MaxWidth(100f));
					EditorGUILayout.PropertyField(vName, GUIContent.none);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();

					EditorGUILayout.BeginHorizontal();
					// Variable target fields.
					EditorGUILayout.LabelField("Target", GUILayout.MaxWidth(50f));
					EditorGUILayout.PropertyField(vTarget, GUIContent.none, GUILayout.MaxWidth(160f));

					// Get reference to the GameObject in the target slot.
					GameObject goTest = vTarget.objectReferenceValue as GameObject;

					// If there is a GameObject in the target field then show a popup of the available variables.
					if(goTest != null)
					{
						// Clear the fields from the previous time we clicked the popup menu.
						TryGetFields(goTest);
						
						// Display current GameObject's fields for the user to select.
						string[] theList = fields.ToArray();
						
						// Popup menu of fields.
						EditorGUILayout.LabelField("Variable", GUILayout.MaxWidth(50f));
						helperIndex.intValue = EditorGUILayout.Popup(helperIndex.intValue, theList);
						// Save the name of the field.

						// TODO Can only select if type is of INT or STRING.
						if(fieldInfos[helperIndex.intValue].FieldType == typeof(string))
						{
							vVariable.stringValue = theList[helperIndex.intValue];
							vBool.boolValue = false;
						}
						else if(fieldInfos[helperIndex.intValue].FieldType == typeof(int))
						{
							vVariable.stringValue = theList[helperIndex.intValue];
							vBool.boolValue = true;
						}
					}
					EditorGUILayout.EndHorizontal();

					// Show clearly what the variable is and what it references.
					if(vTarget.objectReferenceValue != null && vName.stringValue != string.Empty)
					{
						StringBuilder builder = new StringBuilder();
						builder.Append("Variable '" + vName.stringValue + "' references '" + vVariable.stringValue + "' on ");
						builder.Append(vTarget.objectReferenceValue.ToString());
						builder.Replace("(UnityEngine.GameObject)", "");				// Remove this crud to make the message a little cleaner.
						EditorGUILayout.HelpBox(builder.ToString(), MessageType.Info);	// Tell the player exactly what has been stored.
					}
					else
					{
						EditorGUILayout.HelpBox("Variable needs a unique identifier - i.e. 'shopkeeperVisitCount'", MessageType.Warning);	// Tell the player exactly what has been stored.
					}
				}
				// NEW!
				GUILayout.EndVertical();

			}
		}
		//Apply the changes to our list
		GetTarget.ApplyModifiedProperties();
	}

	


	private void TryGetFields(GameObject go)
	{
		fields.Clear();
		fieldInfos.Clear();
		
		// Get Components.
		MonoBehaviour[] monos = go.GetComponents<MonoBehaviour>();

		// Get Methods on those Components.
		foreach(MonoBehaviour mono in monos)
		{
			// Get the type.
			Type myType = mono.GetType();

			FieldInfo[] myField = myType.GetFields();
			for(int i = 0; i < myField.Length; i++)
			{
				fields.Add(myField[i].Name);
				fieldInfos.Add(myField[i]);
			}
		}
	}
}
