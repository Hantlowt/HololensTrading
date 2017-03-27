using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenColor : MonoBehaviour {

	public void ChangePenColor ()
	{
		DrawPixel.myColor = GetComponent<Image>().color;
	}
}
