using UnityEngine;
using System.Collections;

// Attach this to the game camera and drop in the foreground and background elements you want to scroll.
public class BackgroundScroller : MonoBehaviour
{
	public Transform[] Backgrounds;
	public float ParallaxScale = 5.0f;
	public float ParallaxReductionFactor = 2.0f;
	public float smoothing = 1.0f;

	private Vector3 _lastPosition;


	public void Start() 
	{
		_lastPosition = transform.position;
	}
	
	public void Update()
	{
		float parallax = (_lastPosition.x - transform.position.x) * ParallaxScale;
		
		for(int i = 0; i < Backgrounds.Length; i++) 
		{
			float backgroundTargetPosition = Backgrounds[i].position.x + parallax * (i * ParallaxReductionFactor + 1);
			Vector3 endPos = new Vector3(backgroundTargetPosition, Backgrounds[i].position.y, Backgrounds[i].position.z);
			Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position, endPos, smoothing * Time.deltaTime);
		}
		_lastPosition = transform.position;
	}
}
