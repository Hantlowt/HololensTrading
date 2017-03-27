using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPixel : MonoBehaviour {
	private bool OnDraw;
	private Texture2D texture;
	private Color[] TextureColors;
	private int sizeTexture;
	private Vector2 PreviousPoint;
	public static Color myColor;

	// Use this for initialization
	private void Start () {
		texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
		TextureColors = texture.GetPixels();
		sizeTexture = TextureColors.Length;
		myColor = Color.white;
		if (texture == null)
			throw new System.Exception("no texture for the Whiteboard!");
		else
			CleanWhiteBoard();
		OnDraw = false;
		PreviousPoint = new Vector2(-1, -1);
		myColor = Color.black;
	}

	// Update is called once per frame
	private void Update () {
		if (OnDraw)
		{
			Vector2 NewPoint = SearchImpact();
			if (!(NewPoint.x == -1 || PreviousPoint.x == -1))
			{
				DrawLine(NewPoint, PreviousPoint);
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
		return (new Vector2(-1, -1));
	}

	private void DrawLine (Vector2 Start, Vector2 End)
	{
		int w = (int)(End.x - Start.x);
		int h = (int)(End.y - Start.y);
		int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0 ;
		if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
		if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
		if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
		int longest = System.Math.Abs((int)w) ;
		int shortest = System.Math.Abs((int)h) ;
		if (!(longest > shortest))
		{
			longest = System.Math.Abs((int)h);
			shortest = System.Math.Abs((int)w);
			if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
			dx2 = 0;
		}
		int numerator = longest >> 1 ;
		for (int i = 0; i <= longest; i++)
		{
			int pixel = (int)(Start.x + Start.y * texture.width);
			if (pixel >= 0 && pixel < sizeTexture)
				TextureColors[pixel] = myColor;
			numerator += shortest;
			if (!(numerator < longest))
			{
				numerator -= longest;
				Start.x += dx1;
				Start.y += dy1;
			}
			else
			{
				Start.x += dx2;
				Start.y += dy2;
			}
		}
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

	public void OnMouseDown ()
	{
		print("Onmouse");
		PreviousPoint = SearchImpact();
		OnDraw = true;
	}

	public void OnMouseUp ()
	{
		print("Exitmouse");
		OnDraw = false;
	}

}
