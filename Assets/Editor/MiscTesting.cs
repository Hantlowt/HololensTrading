using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class miscTesting : AssetPostprocessor
{
	
	static miscTesting ()
	{
		EditorApplication.update += Update;
	}

	static void Update ()
	{
		AssetDatabase.Refresh();
	}
}