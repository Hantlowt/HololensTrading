using UnityEngine;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Unity.InputModule;

public class GraphKeywords1 : MonoBehaviour, ISpeechHandler
{
	private Color objectColor;
	public GameObject myGraphChild;

	public void ChangeColorGraphLine (string color)
	{
		GraphLine g = myGraphChild.GetComponent<GraphLine>();
		objectColor = g.GetComponent<MeshRenderer>().material.color;
		if (g.online) //cas connecté au serveur
		{
			SyncGraphLine sync = myGraphChild.transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncGraphLine;
			switch (color.ToLower())
			{
				case "red":
					sync.Color_R.Value = Color.red.r;
					sync.Color_G.Value = Color.red.g;
					sync.Color_B.Value = Color.red.b;
					break;
				case "blue":
					sync.Color_R.Value = Color.blue.r;
					sync.Color_G.Value = Color.blue.g;
					sync.Color_B.Value = Color.blue.b;
					break;
				case "green":
					sync.Color_R.Value = Color.green.r;
					sync.Color_G.Value = Color.green.g;
					sync.Color_B.Value = Color.green.b;
					break;
			}
			Debug.Log("en ligne try change color line");
		}
		else //non connecté changement couleur pour le graph en ligne
		{
			switch (color.ToLower())
			{
				case "red":
					objectColor = Color.red;
					break;
				case "blue":
					objectColor = Color.blue;
					break;
				case "green":
					objectColor = Color.green;
					break;
			}
			Debug.Log("pas en ligne try change color line");
		}
	}

	public void ChangeColorGraphBar (string color)
	{
		GraphBar g = myGraphChild.GetComponent<GraphBar>();
		if (g.online) //cas connecté au serveur
		{
			SyncGraphBar sync = myGraphChild.transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncGraphBar;
			switch (color.ToLower())
			{
				case "red":
					sync.Color_R.Value = Color.red.r;
					sync.Color_G.Value = Color.red.g;
					sync.Color_B.Value = Color.red.b;
					break;
				case "blue":
					sync.Color_R.Value = Color.blue.r;
					sync.Color_G.Value = Color.blue.g;
					sync.Color_B.Value = Color.blue.b;
					break;
				case "green":
					sync.Color_R.Value = Color.green.r;
					sync.Color_G.Value = Color.green.g;
					sync.Color_B.Value = Color.green.b;
					break;
			}

			Debug.Log("en ligne try change color bar");
		}
		else // non connecté changement couleur pour le graph en bar
		{
			switch (color.ToLower())
			{
				case "red":
					g.ColorBar = Color.red;
					break;
				case "blue":
					g.ColorBar = Color.blue;
					break;
				case "green":
					g.ColorBar = Color.green;
					break;
			}
			Debug.Log("pas en ligne try change color bar");
		}
	}

	public void DestroyGameObject ()
	{
		Destroy(gameObject);
	}

	public void OnSpeechKeywordRecognized (SpeechKeywordRecognizedEventData eventData)
	{
		switch (eventData.RecognizedText.ToLower())
		{
			case "close":
				DestroyGameObject();
				break;
			default:
				if (myGraphChild.GetComponent<MeshRenderer>())
					ChangeColorGraphLine(eventData.RecognizedText);
				else
					ChangeColorGraphBar(eventData.RecognizedText);
				break;
		}
		print("vous avez dis :" + eventData.RecognizedText);
	}
}
