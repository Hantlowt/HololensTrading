using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadTextureSaved : MonoBehaviour {

	private Texture2D Board;
	private string path;

	private void Start()
	{
		Board = transform.parent.parent.parent.gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
		path = Application.dataPath + "/../Assets/Resources/Boards/";
	}

	public void LoadPNG (Text TextureLabel)
	{
		byte[] file = File.ReadAllBytes(path + TextureLabel.text + ".png");
		Board.LoadImage(file);
		Board.Apply();
	}
}
