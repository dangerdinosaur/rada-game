using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameCounter : MonoBehaviour
{
	int frameCount = 0;
	float dt = 0.0f;
	float fps = 0.0f;
	float updateRate = 4.0f;  // 4 updates per sec.
	private Text counterLabel;


	void Awake()
	{
		counterLabel = GetComponent<Text>();
	}


	void Update()
	{
		frameCount++;
		dt += Time.deltaTime;

		if(dt > 1.0f/updateRate)
		{
			fps = frameCount / dt;
			frameCount = 0;
			dt -= 1.0f/updateRate;

			counterLabel.text = "FPS: " + fps.ToString("F");

//			counterLabel.text = string.Format("FPS: {2:F2}", fps);
		}
	}
}
