using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BresenhamLike : MonoBehaviour {

	public static void DrawLine (Vector2 Start, Vector2 End, int width, Color[] TabColors, Color myColor)
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
			int pixel = (int)(Start.x + Start.y * width);
			if (pixel >= 0 && pixel < TabColors.Length)
				TabColors[pixel] = myColor;
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
}
