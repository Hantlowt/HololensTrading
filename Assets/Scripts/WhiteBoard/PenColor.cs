using UnityEngine;
using UnityEngine.UI;

public class PenColor : MonoBehaviour {

	public void ChangePenColor ()
	{
		DrawPixel.ColorToDraw = GetComponent<Image>().color;
	}
}
