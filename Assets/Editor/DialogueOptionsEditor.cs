using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueOptions))]
public class DialogueOptionsEditor : Editor
{
	// The object of type DialogueOptions this editor is representing.
	private SerializedObject m_optionsData;
	// Properties representing public variables of the DO class.

	// Input
	private SerializedProperty o_skipKey;

	// Audio
	private SerializedProperty o_useAudio;
	private SerializedProperty o_timeBetweenLines;
	private SerializedProperty o_timeBetweenSpeakers;
	private SerializedProperty o_dialogueAudioPath;

	private SerializedProperty o_displayDialogue;
	private SerializedProperty o_clearDialogue;
	private SerializedProperty o_displayTime;

	private SerializedProperty o_responsePrefix;
	private SerializedProperty o_response_HighlightFirst;
	private SerializedProperty o_responseBehaviour;

	private SerializedProperty o_subtitles;
	private SerializedProperty o_subtitleColor;
	private SerializedProperty o_subtitleFont;
	private SerializedProperty o_subtitlePrefix;

	public void OnEnable()
	{
		// Initialise the tagrte object.
		m_optionsData = new SerializedObject(target);

		// Initialise properties - find by the name of the varialbe.

		// Input
		o_skipKey = m_optionsData.FindProperty("skipKey");
		
		// Audio
		o_useAudio = m_optionsData.FindProperty("useAudio");
		o_timeBetweenLines = m_optionsData.FindProperty("timeBetweenLines");
		o_timeBetweenSpeakers = m_optionsData.FindProperty("timeBetweenSpeakers");
		o_dialogueAudioPath = m_optionsData.FindProperty("dialogueAudioPath");

		o_displayDialogue = m_optionsData.FindProperty("displayDialogue");
		o_clearDialogue = m_optionsData.FindProperty("clearDialogue");
		o_displayTime = m_optionsData.FindProperty("displayTime");

		o_responsePrefix = m_optionsData.FindProperty("responsePrefix");
		o_response_HighlightFirst = m_optionsData.FindProperty("response_HighlightFirst");
		o_responseBehaviour = m_optionsData.FindProperty("responseBehaviour");

		o_subtitles = m_optionsData.FindProperty("subtitles");
		o_subtitleColor = m_optionsData.FindProperty("subtitleColor");
		o_subtitleFont = m_optionsData.FindProperty("subtitleFont");
		o_subtitlePrefix = m_optionsData.FindProperty("subtitlePrefix");
	}

	public override void OnInspectorGUI()
	{
		m_optionsData.Update();

		GUILayout.BeginVertical(EditorStyles.textArea);

		GUILayout.Label("Input Options", EditorStyles.toolbarButton);
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(o_skipKey);
		EditorGUILayout.Space();

		o_useAudio.boolValue = EditorGUILayout.Toggle("Use Audio", o_useAudio.boolValue);

		EditorGUILayout.Space();

		if(o_useAudio.boolValue == true)
		{
			// TODO Make it clear it's 'Resources/' - Label in front!
			EditorGUILayout.PropertyField(o_dialogueAudioPath);
		}

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Time Between Lines");
		o_timeBetweenLines.floatValue = EditorGUILayout.Slider(o_timeBetweenLines.floatValue, 0, 5);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Time Between Speakers");
		o_timeBetweenSpeakers.floatValue = EditorGUILayout.Slider(o_timeBetweenSpeakers.floatValue, 0, 5);
		GUILayout.EndHorizontal();

		EditorGUILayout.Space();
		GUILayout.Label("Dialogue Options", EditorStyles.toolbarButton);
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(o_displayDialogue);
		EditorGUILayout.PropertyField(o_clearDialogue);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Display Time");
		o_displayTime.floatValue = EditorGUILayout.Slider(o_displayTime.floatValue, 0, 10);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		GUILayout.Label("Response Options", EditorStyles.toolbarButton);
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(o_responsePrefix);
		EditorGUILayout.PropertyField(o_responseBehaviour);
		o_response_HighlightFirst.boolValue = EditorGUILayout.Toggle("Hightlight First", o_response_HighlightFirst.boolValue);

		EditorGUILayout.Space();
		GUILayout.Label("Subtitle Options", EditorStyles.toolbarButton);
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(o_subtitles);
		if(o_subtitles.enumValueIndex > 0)
		{
			EditorGUILayout.PropertyField(o_subtitleColor);
			EditorGUILayout.PropertyField(o_subtitleFont);
			EditorGUILayout.PropertyField(o_subtitlePrefix);
		}
		EditorGUILayout.Space();
		GUILayout.EndVertical();

		// Apply changes.
		m_optionsData.ApplyModifiedProperties();
	}
}
