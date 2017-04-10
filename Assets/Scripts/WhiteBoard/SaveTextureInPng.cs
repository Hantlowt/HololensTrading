using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveTextureInPng : MonoBehaviour {

	private Texture2D tex;
	private int indexImageSaved;

	// Use this for initialization
	void Start () {
		indexImageSaved = 0;
		tex = transform.parent.parent.parent.gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
	}

	public void OnMouseDown ()
	{
		byte[] bytes = tex.EncodeToPNG();
		string path = Application.dataPath + "./Resources/Texture/";
		string nameImage = "";
		nameImage = path + "BoardSaved" + indexImageSaved.ToString() + ".png";
		// For testing purposes, also write to a file in the project folder
		while (File.Exists(nameImage))
		{
			nameImage = path + indexImageSaved.ToString() + ".png";
			indexImageSaved++;
		}
		File.WriteAllBytes(nameImage, bytes);
	}
}
