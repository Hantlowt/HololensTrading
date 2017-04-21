using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigKeyboardDraw : MonoBehaviour {

	public static int LetterCursorBlank = 36;
	public static int LetterCursorIndex = 37;
	public static Dictionary<KeyCode,int> LetterIndex;
	private static bool First = true;

	void Start()
	{
		if (First)
		{
			First = false;
			LetterIndex = new Dictionary<KeyCode, int>()
			{
			};
			int i2 = 0;
			for (int i = 0; KeyCode.A + i != KeyCode.Z + 1; i++)
			{
				LetterIndex.Add((KeyCode.A + i), i);
				i2++;
			}
			for (int i = 0; KeyCode.Alpha0 + i != KeyCode.Alpha9 + 1; i++)
			{
				LetterIndex.Add((KeyCode.Alpha0 + i), i2 + i);
			}
		}
	}
}