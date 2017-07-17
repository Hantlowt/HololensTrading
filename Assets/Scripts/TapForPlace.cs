using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Unity;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;

public class TapForPlace : MonoBehaviour, IInputHandler {

    public TextMesh text;

    public void Place_Element()
    {
        if (GetComponent<SimpleTagalong>().enabled)
        {
            GetComponent<SimpleTagalong>().enabled = false;
            GetComponent<SharePosition>().receive_data = true;
            SyncSpawnedObject sync = gameObject.GetComponent<DefaultSyncModelAccessor>().SyncModel
                    as SyncSpawnedObject;
            sync.Position.Value = gameObject.transform.position;
            sync.Rotation.Value = gameObject.transform.rotation;
            sync.Scale.Value = gameObject.transform.localScale;
        }
    }

    public void OnMouseUp()
    {
        Place_Element();
    }

    public void OnInputDown(InputEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnInputUp(InputEventData eventData)
    {
        Place_Element();
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<SimpleTagalong>().enabled)
        {
            GetComponent<SharePosition>().receive_data = false;
            text.text = "Tap To Place";
        }
        else
            text.text = "";
    }
}
