using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPixel : MonoBehaviour {
	public static Texture2D Texture;
	public static Color[] TextureColors;
	public static Color ColorToDraw;
	public static Color ColorToErase = Color.white;
	public static Color ColorToDrawPrevious;
	public static int SizePencil;

	public static bool PencilMode;
	public static bool RubberMode;
	private bool OnDraw;
	private Vector2 PreviousPoint;
	private Vector2 VectorNull = new Vector2(0,0);

	// Use this for initialization
	private void Start () {
		PencilMode = false;
		RubberMode = false;
		OnDraw = false;
		ColorToDraw = Color.black;
		SizePencil = 2;
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

	public void ActivePencilMode ()
	{
		if (!PencilMode)
		{
			PencilMode = true;
			ColorToDraw = ColorToDrawPrevious;
			SizePencil = 2;
			DrawLetter.KeyboardMode = false;
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
			DrawLetter.KeyboardMode = false;
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
	/*
	public void ChangeColorToErase ()
	{
		if (ColorToDraw != ColorToErase)
		{
			ColorToDrawPrevious = ColorToDraw;
			ColorToDraw = ColorToErase;
			SizePencil = 10;
		}
		else
		{
			ColorToDraw = ColorToDrawPrevious;
			SizePencil = 2;
		}
	}*/

	public void OnMouseDown ()
	{
		if (PencilMode || RubberMode)
		{
			print("Onmouse");
			PreviousPoint = SearchImpact();
			OnDraw = true;
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
