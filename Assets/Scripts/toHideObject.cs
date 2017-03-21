using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class toHideObject : MonoBehaviour, ISpeechHandler
{

	public void Hide (GameObject myObject)
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
			case "hide":
				Hide(gameObject);
				break;
		}
	}
}
