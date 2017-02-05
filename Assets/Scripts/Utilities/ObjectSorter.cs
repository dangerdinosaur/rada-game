using UnityEngine;
using System.Collections;

public class ObjectSorter : MonoBehaviour
{
	SpriteRenderer spriteRenderer;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void LateUpdate()
	{
		spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint(spriteRenderer.bounds.min).y * -1;
	}
}
