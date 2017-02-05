using UnityEngine;
using System.Collections;

public class TextManager : MonoBehaviour
{
	// Static instance of GameManager which allows it to be accessed by any other script.
	public static TextManager instance = null;
	// A reference to the text database. Could load this with Resources.Load?
	// This is what you'd swap out with another language. So use the French database instead.
	// TODO Function to swap out languages.
	public SimpleTextDatabase Database
	{
		get { return _database; }
	}

	private SimpleTextDatabase _database;
	
	public enum Language { English, French, Pirate, Custom };


	// Awake is always called before any Start functions
	void Awake()
	{
		// Check if instance already exists
		if(instance == null)
			// if not, set instance to this
			instance = this;
		
		// If instance already exists and it's not this:
		else if(instance != this)
			// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		
		// Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		// Load default language...
		ChangeLanguage_English();
	}


	public string GetText(int id)
	{
		if(_database.lines.Count >= id)
		{
			return _database.lines[id].lineText;
		}
		else
			return "MISSING TEXT";
	}

	#region CHANGE LANGUAGES
	public void ChangeLanguage(Language lang)
	{
		// Load language.
		_database = Utilities.Load<SimpleTextDatabase>("SimpleTextDatabase_" + lang);
	}

	public void ChangeLanguage_English()
	{
		ChangeLanguage(Language.English);
	}

	public void ChangeLanguage_French()
	{
		ChangeLanguage(Language.French);
	}

	public void ChangeLanguage_Pirate()
	{
		ChangeLanguage(Language.Pirate);
	}

	public void ChangeLanguage_Custom()
	{
		ChangeLanguage(Language.Custom);
	}
	#endregion
}
