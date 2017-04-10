using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class FindAndDisplayTextureList : MonoBehaviour {
	private Dropdown dropDown;
	private List<string> options;

	// Use this for initialization
	void Start ()
	{
		DisplayTextureList();
	}

	public void DisplayTextureList()
	{
		string path = Application.dataPath + "/Resources/Texture/";
		string pattern = @"[\\\/]([^.\\\/]+)\.\w+$";
		string[] files;

		files = System.IO.Directory.GetFiles(path, "*.png");

		dropDown = GetComponent<Dropdown>();
		dropDown.ClearOptions();

		options = new List<string>();
		foreach (string fileName in files)
		{
			options.Add(Regex.Match(fileName, pattern, RegexOptions.Multiline).Groups[1].Value);
		}
		dropDown.AddOptions(options);
	}
}
