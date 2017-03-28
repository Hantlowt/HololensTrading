using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPixel : MonoBehaviour {
	public static Texture2D Texture;
	public static Color[] TextureColors;
	public static Color ColorToDraw;
	public static Color ColorToErase = Color.white;
	

	public Sprite[] Letters;

	private bool PencilMode;
	private bool RubberMode;
	private bool KeyboardMode;
	private bool OnDraw;
	private bool OnTape;
	private Color ColorToDrawPrevious;
	private int SizePencil;
	private Vector2 PreviousPoint;
	private Vector2 CursorCoord;
	private Vector2 VectorNull = new Vector2(0,0);

	// Use this for initialization
	private void Start () {
		PencilMode = false;
		RubberMode = false;
		KeyboardMode = false;
		OnTape = false;
		OnDraw = false;
		ColorToDraw = Color.black;
		SizePencil = 2;
		CursorCoord = VectorNull;
		Texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
		TextureColors = Texture.GetPixels();
		if (Texture == null)
			throw new System.Exception("no texture for the Whiteboard!");
		else
			CleanWhiteBoard();
	}

	// Update is called once per frame
	private void Update () {
		if (OnDraw)
		{
			Vector2 NewPoint = SearchImpact();
			if (NewPoint != VectorNull && PreviousPoint != VectorNull)
			{
				BresenhamLike.DrawLineWithSize(SizePencil, NewPoint, PreviousPoint, Texture.width, TextureColors, ColorToDraw);
				Texture.SetPixels(TextureColors);
				Texture.Apply();
				print("hit ok");
			}
			PreviousPoint = NewPoint;
		}
		else if (OnTape)
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

	private Vector2 SearchImpact()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100))
		{
			print("hit =" + (int)(hit.textureCoord2.x * Texture.width) + " , " + (int)(hit.textureCoord2.y * Texture.height));
			print("hit2 =" + (int)(hit.textureCoord2.x) + " , " + (int)(hit.textureCoord2.y ));
			int x = (int)(hit.textureCoord.x * Texture.width);
			int y = (int)(hit.textureCoord.y * Texture.height);
			//return (x + y * texture.width);
			return (new Vector2(x, y));
		}
		return (VectorNull);
	}

	private void DrawLetterATest ()
	{
		print("A is pressed");
		Texture2D letters_tex = Letters[0].texture;
		Color[] letter_A = letters_tex.GetPixels((int)Letters[1].rect.x, (int)Letters[1].rect.y, (int)Letters[1].rect.width, (int)Letters[1].rect.height);
		for (int i = 0; i < TextureColors.Length; i++)
		{
			TextureColors[i] = Color.magenta;
		}
		print("x=" + (int)CursorCoord.x + ", y=" + (int)CursorCoord.y + "width=" + (int)Letters[0].rect.width + "height=" + (int)Letters[0].rect.height + "et texture.width = " + Texture.width);
		Texture.SetPixels((int)CursorCoord.x, (int)CursorCoord.y, (int)Letters[1].rect.width, (int)Letters[1].rect.height, letter_A);
		Texture.Apply();
	}

	private IEnumerator ActiveCursor ()
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
			OnDraw = false;
			PencilMode = false;
			RubberMode = false;
		}
		else
		{
			StopCoroutine("ActiveCursor");
			KeyboardMode = false;
			OnTape = false;
		}
	}

	public void ActivePencilMode ()
	{
		if (!PencilMode)
		{
			PencilMode = true;
			ColorToDraw = ColorToDrawPrevious;
			SizePencil = 2;
			KeyboardMode = false;
			OnTape = false;
			RubberMode = false;
		}
		else
			PencilMode = false;
	}

	public void ActiveRubberMode ()
	{
		if (!RubberMode)
		{
			RubberMode = true;
			ColorToDrawPrevious = ColorToDraw;
			ColorToDraw = ColorToErase;
			SizePencil = 10;
			OnTape = false;
			KeyboardMode = false;
			PencilMode = false;
		}
		else
			RubberMode = false;
	}

	public void CleanWhiteBoard ()
	{
		for (int i = 0; i < TextureColors.Length; i++)
		{
			TextureColors[i] = ColorToErase;
		}
		Texture.SetPixels(TextureColors);
		Texture.Apply();
	}

	public void OnMouseDown ()
	{
		print("Onmouse");
		if (PencilMode || RubberMode)
		{
			print("On Pencil or Rubber mode");
			PreviousPoint = SearchImpact();
			OnDraw = true;
		}
		else if (KeyboardMode)
		{
			print("OnKeyboardMode");
			CursorCoord = SearchImpact();
			//StartCoroutine("ActiveCursor");
			OnTape = true;
		}
	}

	public void OnMouseUp ()
	{
		if (PencilMode || RubberMode)
		{
			print("Exitmouse");
			OnDraw = false;
		}
	}
}
