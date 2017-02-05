using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Conversation))]
[CanEditMultipleObjects]
public class ConversationEditor : Editor
{
	private Conversation _convo;
	private string[] parseTypes = new string[] { "Auto-Update Mode", "Manual Update Mode" };
	private int parseTypeIndex = 0;


	void Awake()
	{
		_convo = (Conversation)target;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.BeginVertical(EditorStyles.textArea);
		GUILayout.Label("Conversation File", EditorStyles.toolbarButton);

		// Show the text source file property.
		EditorGUILayout.PropertyField(serializedObject.FindProperty("sourceFile"), true);

		// Show a warning if there's no source file.
		if(_convo.sourceFile == null)
		{
			EditorGUILayout.HelpBox("There is no text file to parse - drag one into the box above.", MessageType.Warning);
		}

		EditorGUILayout.Space();

		// Auto-update selection.
		// 'Auto-Update Mode' is for development - when you are making lots of changes to the source file.
		// 'Manual Update Mode' is when you are content-locked and ready to compile your game.
		parseTypeIndex = EditorGUILayout.Popup("Update Mode", parseTypeIndex, parseTypes, EditorStyles.popup);

		EditorGUILayout.Space();

		if(parseTypeIndex == 0)
		{
			// Auto-update Mode.
			_convo.autoUpdate = true;
		}
		else if(parseTypeIndex == 1)
		{
			// Manual Update Mode.
			_convo.autoUpdate = false;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Manual Update");
			if(GUILayout.Button("Update Conversation", EditorStyles.miniButton))
			{
				// Parse and store the conversation.
				_convo.UpdateConversation();
				//tell the unity editor that the list has changed and needs to be serialized
				EditorUtility.SetDirty(target);
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space();

		// Help box to let you know which mode you are currently using and a decription of what it is for.
		if(_convo.autoUpdate)
		{
			EditorGUILayout.HelpBox("Auto-update mode: The source file will be parsed at runtime. This mode is good for development when you are making regular changes to the source file.", MessageType.Info);
		}
		else if(!_convo.autoUpdate)
		{
			EditorGUILayout.HelpBox("Manual update mode: The source file will not be parsed at runtime. Click the 'Update Conversation' button above to update the conversation if you have made any changes to the source file.", MessageType.Info);
		}

		EditorGUILayout.Space();

		if(_convo.sourceFile != null && _convo.dialogSets.titles.Count > 0)
		{
			EditorGUILayout.HelpBox("Conversation contains " + _convo.dialogSets.titles.Count + " dialogue sets with the following titles:", MessageType.None);

			for(int i = 0; i < _convo.dialogSets.titles.Count; i++)
			{
				EditorGUILayout.HelpBox((i + 1) + ". " + _convo.dialogSets.titles[i], MessageType.None);
			}
		}

		EditorGUILayout.Space();

		// Help box that displays the date and time that the conversation was last updated.
		if(_convo.lastUpdated == "")
		{
			EditorGUILayout.HelpBox("Source file has not been processed yet.", MessageType.Warning);
		}
		else if(_convo.lastUpdated != "")
		{
			EditorGUILayout.HelpBox("Conversation was last updated: " + _convo.lastUpdated, MessageType.None);
		}

		EditorGUILayout.EndVertical();

		// Apply changes.
		serializedObject.ApplyModifiedProperties();
	}	
}
