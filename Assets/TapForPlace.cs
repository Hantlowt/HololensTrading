using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Unity;

public class TapForPlace : MonoBehaviour, IInputHandler {

    public void OnInputDown(InputEventData eventData)
    {
        GetComponent<SimpleTagalong>().enabled = false;
        GetComponent<SharePosition>().receive_data = true;
    }

    public void OnInputUp(InputEventData eventData)
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        GetComponent<SharePosition>().receive_data = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
