using System.IO;
using UnityEngine;

public class SaveTextureInPng : MonoBehaviour
{

	private Texture2D tex;
	private string path;
	
	void Start ()
	{
		tex = transform.parent.parent.parent.gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
		path = Application.dataPath + "/../Assets/Resources/Boards/";
	}

	public void OnMouseDown ()
	{
		int indexImageSaved = 0;
		byte[] bytes = tex.EncodeToPNG();
		string NewNameFile = "Board" + indexImageSaved.ToString();

		while ((Resources.Load<Texture2D>("Boards/" + NewNameFile)) != null)
		{
			indexImageSaved++;
			NewNameFile = "Board" + indexImageSaved.ToString();
		}
		File.WriteAllBytes(path + NewNameFile + ".png", bytes);
		print("save to" + path + NewNameFile + ".png");
	}
}
