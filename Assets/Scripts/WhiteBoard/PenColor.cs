using UnityEngine;
using UnityEngine.UI;

public class PenColor : MonoBehaviour {
	public GameObject PencilSprite;

	public void ChangePenColor ()
	{
		if (DrawPixel.ColorToDraw != Color.white) // si la couleur pour effacer n'est pas sélectionner
		{
			DrawPixel.ColorToDraw = GetComponent<Image>().color;
			PencilSprite.GetComponent<SpriteRenderer>().color = GetComponent<Image>().color;
		}
	}
}