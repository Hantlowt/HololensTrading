using UnityEngine;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Unity.InputModule;

public class GraphKeywords1 : MonoBehaviour, ISpeechHandler
{
	public GameObject myGraphChild;

	public void ChangeColorGraphLine (string color)
	{
		GraphLine g = myGraphChild.GetComponent<GraphLine>();
		Color objectColor = g.GetComponent<MeshRenderer>().material.color;
		if (g.online) //cas connecté au serveur
		{
			SyncGraphLine sync = myGraphChild.transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncGraphLine;
			switch (color)
			{
				case "red":
					{
						sync.Color_R.Value = Color.red.r;
						sync.Color_G.Value = Color.red.g;
						sync.Color_B.Value = Color.red.b;
						break;
					}
				case "blue":
					{
						sync.Color_R.Value = Color.blue.r;
						sync.Color_G.Value = Color.blue.g;
						sync.Color_B.Value = Color.blue.b;
						break;
					}
				case "green":
					{
						sync.Color_R.Value = Color.green.r;
						sync.Color_G.Value = Color.green.g;
						sync.Color_B.Value = Color.green.b;
						break;
					}
				case "yellow":
					{
						sync.Color_R.Value = Color.yellow.r;
						sync.Color_G.Value = Color.yellow.g;
						sync.Color_B.Value = Color.yellow.b;
						break;
					}
			}
		}
		else //non connecté changement couleur pour le graph en ligne
		{
			switch (color)
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
				case "yellow":
					objectColor = Color.yellow;
					break;
			}
		}
	}

	public void ChangeColorGraphBar (string color)
	{
		GraphBar g = myGraphChild.GetComponent<GraphBar>();
		if (g.online) //cas connecté au serveur
		{
			SyncGraphBar sync = myGraphChild.transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncGraphBar;
			switch (color)
			{
				case "red":
					{
						sync.Color_R.Value = Color.red.r;
						sync.Color_G.Value = Color.red.g;
						sync.Color_B.Value = Color.red.b;
						break;
					}
				case "blue":
					{
						sync.Color_R.Value = Color.blue.r;
						sync.Color_G.Value = Color.blue.g;
						sync.Color_B.Value = Color.blue.b;
						break;
					}
				case "green":
					{
						sync.Color_R.Value = Color.green.r;
						sync.Color_G.Value = Color.green.g;
						sync.Color_B.Value = Color.green.b;
						break;
					}
				case "yellow":
					{
						sync.Color_R.Value = Color.yellow.r;
						sync.Color_G.Value = Color.yellow.g;
						sync.Color_B.Value = Color.yellow.b;
						break;
					}
			}
		}
		else // non connecté changement couleur pour le graph en bar
		{
			switch (color)
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
				case "yellow":
					g.ColorBar = Color.yellow;
					break;
			}
		}
	}

	public void DestroyGameObject (GameObject myObject)
	{
		Destroy(myObject, 0.1f);
	}

	public void HideObject (GameObject myObject)
	{
		if (myObject.activeSelf)
			myObject.SetActive(false);
		else
			myObject.SetActive(true);
	}

	public void OnSpeechKeywordRecognized (SpeechKeywordRecognizedEventData eventData)
	{
		print("vous avez dis :" + eventData.RecognizedText);
		switch (eventData.RecognizedText.ToLower())
		{
			case "close":
				DestroyGameObject(gameObject);
				break;
			case "hide":
				HideObject(myGraphChild);
				break;
			default:
				{
					if (myGraphChild.GetComponent<MeshRenderer>())
						ChangeColorGraphLine(eventData.RecognizedText.ToLower());
					else
						ChangeColorGraphBar(eventData.RecognizedText.ToLower());
					break;
				}
		}
	}
}