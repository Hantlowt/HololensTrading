using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLetter : MonoBehaviour {
	public static bool KeyboardMode;
	// Use this for initialization
	void Start () {
		KeyboardMode = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActiveKeyboardMode ()
	{
		KeyboardMode = !KeyboardMode;
		DrawPixel.PencilMode = false;
	}
}
