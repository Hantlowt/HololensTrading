using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        if (File.Exists(Application.persistentDataPath + "/boards.save"))
        {
            string[] all_file = File.ReadAllLines(Application.persistentDataPath + "/boards.save");
            foreach(string line in all_file)
            {
                options.Add("Board" + indexImageSaved.ToString());
                indexImageSaved++;
            }
        }
          /*  while ((Resources.Load<Texture2D>("Boards/" + "Board" + indexImageSaved.ToString())) != null)
		{
			options.Add("Board" + indexImageSaved.ToString());
			indexImageSaved++;
		}*/
		dropDown.AddOptions(options);

		//print("update check list index = " + indexImageSaved);
	}
}
