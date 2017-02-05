using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SimpleTextDatabase))]
[CanEditMultipleObjects]
public class TextDatabaseEditor : Editor
{
	private SimpleTextDatabase _database;


	void Awake()
	{
		_database = (SimpleTextDatabase)target;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.BeginVertical(EditorStyles.textArea);
		GUILayout.Label("Text Database", EditorStyles.toolbarButton);

		// Show the text source file property.
		EditorGUILayout.PropertyField(serializedObject.FindProperty("sourceFile"), true);

		// Show a warning if there's no source file.
		if(_database.sourceFile == null)
		{
			EditorGUILayout.HelpBox("There is no text file to parse - drag one into the box above.", MessageType.Warning);
		}

		EditorGUILayout.Space();

		// Manual Update Mode.
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Manual Update");

		if(GUILayout.Button("Update Database", EditorStyles.miniButton))
		{
			// Parse and store the conversation.
			_database.UpdateDatabase();
			//tell the unity editor that the list has changed and needs to be serialized
			EditorUtility.SetDirty(target);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		// Help box to let you know which mode you are currently using and a decription of what it is for.
		EditorGUILayout.HelpBox("Remeber to click the 'Update Conversation' button above to update the conversation if you have made any changes to the source file.", MessageType.Info);

		EditorGUILayout.Space();

		if(_database.sourceFile != null && _database.lines.Count > 0)
		{
			EditorGUILayout.HelpBox("Database contains " + _database.lines.Count + " lines:", MessageType.None);
		}

		EditorGUILayout.Space();

		// Help box that displays the date and time that the conversation was last updated.
		if(_database.lastUpdated == "")
		{
			EditorGUILayout.HelpBox("Source file has not been processed yet.", MessageType.Warning);
		}
		else if(_database.lastUpdated != "")
		{
			EditorGUILayout.HelpBox("Database was last updated: " + _database.lastUpdated, MessageType.None);
		}

		EditorGUILayout.EndVertical();

		// Apply changes.
		serializedObject.ApplyModifiedProperties();
	}	
}
