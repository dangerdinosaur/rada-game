using UnityEngine;
using UnityEditor;

public class ut_CreateAssets
{
	[MenuItem("Assets/Create/Conversation Asset")]
	public static void CreateAsset()
	{
		CustomAssetUtility.CreateAsset<Conversation>();
	}

	[MenuItem("Assets/Create/Dialogue Options Asset")]
	public static void CreateDialoguePreset()
	{
		CustomAssetUtility.CreateAsset<DialogueOptions>();
	}

	[MenuItem("Assets/Create/Simple Text Database Asset")]
	public static void CreateSimpleTextDatabaseAsset()
	{
		CustomAssetUtility.CreateAsset<SimpleTextDatabase>();
	}
}
