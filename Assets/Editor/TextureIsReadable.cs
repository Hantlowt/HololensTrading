using UnityEngine;
using UnityEditor;
using System.Collections;

public class TextureIsReadable : MonoBehaviour{
	/*
	// Use this for initialization
	void Start () {
			print("coucou");
			StartCoroutine("UpdateSprite");
		}


	// Update is called once per frame
	private IEnumerator UpdateSprite () {
		int indexImageSaved = 0;
		Texture2D texture;

		print("coucoBInu");
		while ((texture = Resources.Load<Texture2D>("Boards/" + "Board" + indexImageSaved.ToString())) != null)
		{
			print("coucouplop");
			SetTextureReadable(texture);
			indexImageSaved++;
		}
		AssetDatabase.Refresh();
		yield return new WaitForSecondsRealtime(2);
	}

	public static void SetTextureReadable (Texture2D texture)
	{
		string assetPath = AssetDatabase.GetAssetPath(texture);
		print(assetPath);
		TextureImporter tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
		if (tImporter != null)
		{
			tImporter.textureType = TextureImporterType.Default;
			tImporter.isReadable = true;
			AssetDatabase.ImportAsset(assetPath);
			AssetDatabase.Refresh();
		}
	}
	*/
}
