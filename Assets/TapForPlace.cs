using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Unity;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;

public class TapForPlace : MonoBehaviour, IInputHandler {

    public void OnInputDown(InputEventData eventData)
    {
        GetComponent<SimpleTagalong>().enabled = false;
        GetComponent<SharePosition>().receive_data = true;
        SyncSpawnedObject sync = gameObject.GetComponent<DefaultSyncModelAccessor>().SyncModel
                as SyncSpawnedObject;
        sync.Position.Value = gameObject.transform.position;
        sync.Rotation.Value = gameObject.transform.rotation;
        sync.Scale.Value = gameObject.transform.localScale;
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
