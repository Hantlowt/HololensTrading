using UnityEngine;
using UnityEngine.EventSystems;
using HoloToolkit.Unity.InputModule;

public class Move : EventTrigger, ISpeechHandler
{
    HandsTrackingManager input;
    Selector s;

    public void Start()
    {
        Debug.Log("Start button");
        input = GameObject.Find("InputManager").GetComponent<HandsTrackingManager>();
        s = GameObject.Find("Cursor").GetComponent<Selector>();
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        transform.parent.parent.position += input.velocity / 30.0f;
    }

    public override void OnPointerDown(PointerEventData data)
    {
        s.enable_disable(this.transform.parent.parent.gameObject);
        Debug.Log("OnPointerDown called.");
    }

    public override void OnPointerUp(PointerEventData data)
    {
        s.enable_disable(null);
        Debug.Log("OnPointerUp called.");
    }

	public void SayMove ()
	{
		s.enable_disable(this.transform.parent.parent.gameObject);
	}

	public void SayPlace ()
	{
		s.enable_disable(null);
	}
	public void OnSpeechKeywordRecognized (SpeechKeywordRecognizedEventData eventData)
	{
		switch(eventData.RecognizedText.ToLower())
		{
			case "move":
				SayMove();
				break;
			case "place":
				SayPlace();
				break;
		}
	}

}