using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadTextureSaved : MonoBehaviour {

	private Texture2D Board;
	private Texture2D tex;

	private void Start()
	{
		Board = transform.parent.parent.parent.gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
	}

	public void LoadPNG (Text TextureLabel)
	{
		string fileChoosen;
		byte[] fileData;

		fileChoosen = Application.dataPath + "./Resources/Texture/" + TextureLabel.text + ".png";
			print (fileChoosen);
			if (File.Exists(fileChoosen))
			{
				print ("PAF");
				fileData = File.ReadAllBytes(fileChoosen);
				tex = new Texture2D (2, 2);
				tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
				//transform.parent.parent.parent.gameObject.GetComponent<Renderer>().material.mainTexture = tex;
				Board.SetPixels(tex.GetPixels());
				Board.Apply();
			}
	}
}
