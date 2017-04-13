using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindAndDisplayTextureList : MonoBehaviour {
	private Dropdown dropDown;
	private List<string> options;

	void Start ()
	{
		DisplayTextureList();
	}

	public void DisplayTextureList()
	{
		int indexImageSaved = 0;

		dropDown = GetComponent<Dropdown>();
		dropDown.ClearOptions();
		options = new List<string>();
		while ((Resources.Load<Texture2D>("Boards/" + "Board" + indexImageSaved.ToString())) != null)
		{
			options.Add("Board" + indexImageSaved.ToString());
			indexImageSaved++;
		}
		dropDown.AddOptions(options);

		//print("update check list index = " + indexImageSaved);
	}
}
