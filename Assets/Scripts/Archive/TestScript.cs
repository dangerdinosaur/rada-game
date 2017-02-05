using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Base : MonoBehaviour
{
	public int myInt;
}

[Serializable]
public class Derived : Base
{
	public string myString;
}

public class TestScript : MonoBehaviour
{
	public Base _myBase;

	// This gets called in the editor when you first create this monobehavior.
	void Reset()
	{
		_myBase = gameObject.AddComponent<Derived>();
		_myBase.hideFlags = HideFlags.HideInInspector;
	}
}
