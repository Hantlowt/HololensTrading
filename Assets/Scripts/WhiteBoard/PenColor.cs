using UnityEngine;
using UnityEngine.UI;

public class PenColor : MonoBehaviour {

	public void ChangePenColor ()
	{
		if (DrawPixel.ColorToDraw != Color.white) // si la couleur pour effacer n'est pas sélectionner
			DrawPixel.ColorToDraw = GetComponent<Image>().color;
	}
}