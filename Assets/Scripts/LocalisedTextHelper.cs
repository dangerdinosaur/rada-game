using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocalisedTextHelper : MonoBehaviour
{
	public int textID;
	private Text textBox; 

	void Start()
	{
		// Get the reference.
		textBox = GetComponent<Text>();
		// Init it's text.
		textBox.text = TextManager.instance.GetText(textID);
	}
}
