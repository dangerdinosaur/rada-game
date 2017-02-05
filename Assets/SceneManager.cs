using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
	private float _minDuration = 0.25f;

	IEnumerator LoadScene(string sceneName)
	{
		// Fade to black
		yield return StartCoroutine("FadeIn");

		// 'Load' animated loading scene
		// IDEA - could the loding scene be within the game scene and the camera just move to it?
//		yield return Application.LoadLevelAsync("LoadingScene");

		// Fade to loading scene
		yield return StartCoroutine("FadeOut");

		float endTime = Time.time + _minDuration;

		// Load level async
		yield return Application.LoadLevelAdditiveAsync(sceneName);

		if(Time.time < endTime)
			yield return new WaitForSeconds(endTime - Time.time);

		// Fade to black
		yield return StartCoroutine("FadeIn");

		// 'Unload' loading screen.
		// Camera moves back to the main scene position?

		// Fade to new scene.
		yield return StartCoroutine("FadeOut");
	}
}
