using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utilities
{
	// Editor Defaults
	public const float editorSpacerHeight = 4.0f;

	// Game Defaults
	public const float walkSpeed = 2.0f;
	public const float runSpeed = 3.5f;


	// TODO - Get rid of this - only for early prototyping. Look, Take, Use, Use With, Talk, Give
	static string[] responses = new string[]
	{
		"I can't talk to that.",
		"That's not a great source of conversation.",
		"Those aren't known for being great conversationalists.",
		"I don't have time for that."
	};

	// Return a random string from the responses array declared above.
	public static string RandomNegativeTalkResponse()
	{
		// TODO Need to be converted to a 'Response' class function - returning an id and a string etc.
		return responses[Random.Range(0, responses.Length)];
	}


	// Get the heading of an object - returns a Vector 3 'heading'.
	public static Vector3 GetHeadingOfTwoTransforms(Vector3 targetPos, Vector3 objectPosition)
	{
		Vector3 heading = targetPos - objectPosition;
		return heading;
	}


	// Generic 'Load from Resources' function.
	public static T Load<T>(string path) where T : Object
	{
		T thing = (T)Resources.Load (path);

		if(thing == null)
		{
			Debug.LogError("Couldn't load resource of type " + typeof(T) + " with path: " + path);
		}

		return thing;
	}


	// Random Number
	public static int RandomNumber(int rangeLength)
	{
		return Random.Range(0, rangeLength);
	}


	//arrayToCurve is original Vector3 array, smoothness is the number of interpolations. 
	public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve,float smoothness){
		List<Vector3> points;
		List<Vector3> curvedPoints;
		int pointsLength = 0;
		int curvedLength = 0;

		if(smoothness < 1.0f) smoothness = 1.0f;

		pointsLength = arrayToCurve.Length;

		curvedLength = (pointsLength*Mathf.RoundToInt(smoothness))-1;
		curvedPoints = new List<Vector3>(curvedLength);

		float t = 0.0f;
		for(int pointInTimeOnCurve = 0;pointInTimeOnCurve < curvedLength+1;pointInTimeOnCurve++){
			t = Mathf.InverseLerp(0,curvedLength,pointInTimeOnCurve);

			points = new List<Vector3>(arrayToCurve);

			for(int j = pointsLength-1; j > 0; j--){
				for (int i = 0; i < j; i++){
					points[i] = (1-t)*points[i] + t*points[i+1];
				}
			}

			curvedPoints.Add(points[0]);
		}

		return(curvedPoints.ToArray());
	}
}
