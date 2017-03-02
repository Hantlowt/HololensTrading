using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastToChilds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Raycast_Receiver(RaycastHit h)
    {
        foreach (Transform child in transform)
            child.SendMessage("Raycast_Receiver", h, SendMessageOptions.DontRequireReceiver);
    }
}
