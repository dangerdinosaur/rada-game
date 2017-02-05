using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWindowBase : MonoBehaviour, IDragHandler
{
	// Use this for speech bubbles and keeping them on screen. With heavy modification.

	private RectTransform rectTransform;
	private Canvas canvas;
	private RectTransform canvasRectTransform;

	public int keepWindowInCanvas = 5;            // # of pixels of the window that must stay inside the canvas view.


	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>();
		canvasRectTransform = canvas.GetComponent<RectTransform>();
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		var delta = ScreenToCanvas(eventData.position) - ScreenToCanvas(eventData.position - eventData.delta);
		rectTransform.localPosition += delta;
	}
	
	private Vector3 ScreenToCanvas(Vector3 screenPosition)
	{
		Vector3 localPosition;
		Vector2 min;
		Vector2 max;
		var canvasSize = canvasRectTransform.sizeDelta;
		
		if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
		{
			localPosition = screenPosition;
			
			min = Vector2.zero;
			max = canvasSize;
		}
		else
		{
			var ray = canvas.worldCamera.ScreenPointToRay(screenPosition);
			var plane = new Plane(canvasRectTransform.forward, canvasRectTransform.position);
			
			float distance;
			if (plane.Raycast(ray, out distance) == false)
			{
				throw new Exception("Is it practically possible?");
			};
			var worldPosition = ray.origin + ray.direction * distance;
			localPosition = canvasRectTransform.InverseTransformPoint(worldPosition);
			
			min = -Vector2.Scale(canvasSize, canvasRectTransform.pivot);
			max = Vector2.Scale(canvasSize, Vector2.one - canvasRectTransform.pivot);
		}

		// keep window inside canvas
		localPosition.x = Mathf.Clamp(localPosition.x, min.x + keepWindowInCanvas, max.x - keepWindowInCanvas);
		localPosition.y = Mathf.Clamp(localPosition.y, min.y + keepWindowInCanvas, max.y - keepWindowInCanvas);

		
		return localPosition;
	}
}