using System.Collections;
using UnityEngine;

public class DrawLetter : MonoBehaviour{
	public static bool KeyboardMode;
	public Sprite[] Letters;
	private Vector2 VectorNull = new Vector2(0,0);

	private bool OnTape;
	// Use this for initialization
	void Start () {
		KeyboardMode = false;
		OnTape = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (OnTape)
		{
			if (Input.anyKeyDown)
			{
				Debug.Log("A key or mouse click has been detected");
			}
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				StopCoroutine("ActiveCursor");
				OnTape = false;
			}
			if (Input.GetKeyUp(KeyCode.A))
				DrawLetterATest();
		}
	}

	private void DrawLetterATest ()
	{
		print("A is pressed");
		for (int i = 0; i < DrawPixel.TextureColors.Length; i++)
		{
			DrawPixel.TextureColors[i] = Color.magenta;
		}
		DrawPixel.Texture.SetPixels(DrawPixel.TextureColors);
		DrawPixel.Texture.Apply();
	}

	private IEnumerator ActiveCursor()
	{
		while (true)
		{
			print("Je suis pret à saisir vos lettres");
			/* TO DO :
			 * Faire clignoter un curseur
			 * */
			yield return null;
		}
	}

	public void ActiveKeyboardMode ()
	{
		if (!KeyboardMode)
		{
			KeyboardMode = true;
			StopCoroutine("ActiveCursor");
			OnTape = false;
			DrawPixel.PencilMode = false;
			DrawPixel.RubberMode = false;
		}
		else
			KeyboardMode = false;
	}

	public void OnMouseDown ()
	{
		if (KeyboardMode)
		{
			print("OnKeyboardMode");
			StartCoroutine("ActiveCursor");
			OnTape = true;
		}
	}
}
