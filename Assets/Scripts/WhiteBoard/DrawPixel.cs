using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPixel : MonoBehaviour {
	public static bool PencilMode;
	private bool OnDraw;
	private Texture2D texture;
	private Color[] TextureColors;
	private int sizeTexture;
	private Vector2 PreviousPoint;
	private Vector2 VectorNull = new Vector2(0,0);
	public static Color myColor;

	// Use this for initialization
	private void Start () {
		PencilMode = false;
		OnDraw = false;
		texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
		TextureColors = texture.GetPixels();
		sizeTexture = TextureColors.Length;
		myColor = Color.white;
		if (texture == null)
			throw new System.Exception("no texture for the Whiteboard!");
		else
			CleanWhiteBoard();
		PreviousPoint = VectorNull;
		myColor = Color.black;
	}

	// Update is called once per frame
	private void Update () {
		if (OnDraw)
		{
			Vector2 NewPoint = SearchImpact();
			if (NewPoint != VectorNull && PreviousPoint != VectorNull)
			{
				BresenhamLike.DrawLine(NewPoint, PreviousPoint, texture.width, TextureColors, myColor);
				texture.SetPixels(TextureColors);
				texture.Apply();
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
			print("hit =" + (int)(hit.textureCoord2.x * texture.width) + " , " + (int)(hit.textureCoord2.y * texture.height));
			print("hit2 =" + (int)(hit.textureCoord2.x) + " , " + (int)(hit.textureCoord2.y ));
			int x = (int)(hit.textureCoord.x * texture.width);
			int y = (int)(hit.textureCoord.y * texture.height);
			//return (x + y * texture.width);
			return (new Vector2(x, y));
		}
		return (VectorNull);
	}

	public void ChangeColorToErase ()
	{
		myColor = Color.white;
	}

	public void CleanWhiteBoard ()
	{
		for (int i = 0; i < TextureColors.Length; i++)
		{
			TextureColors[i] = Color.white;
		}
		texture.SetPixels(TextureColors);
		texture.Apply();
	}

	public void ActivePencilMode ()
	{
		PencilMode = !PencilMode;
		DrawLetter.KeyboardMode = false;
	}

	public void OnMouseDown ()
	{
		if (PencilMode)
		{
			print("Onmouse");
			PreviousPoint = SearchImpact();
			OnDraw = true;
		}
	}

	public void OnMouseUp ()
	{
		if (PencilMode)
		{
			print("Exitmouse");
			OnDraw = false;
		}
	}
}
