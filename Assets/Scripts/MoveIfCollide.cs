using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;

public class MoveIfCollide : MonoBehaviour {

    SharePosition s;
	// Use this for initialization
	void Start () {
        s = GetComponent<SharePosition>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Graph")
            s.receive_data = false;
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Graph")
            this.transform.localPosition += new Vector3(0.3f, 0.0f, 0.0f);
    }

    void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Graph")
        {
            SyncSpawnedObject sync = GetComponent<DefaultSyncModelAccessor>().SyncModel
                as SyncSpawnedObject;
            sync.Position.Value = transform.position;
            sync.Rotation.Value = transform.rotation;
            sync.Scale.Value = transform.localScale;
            s.receive_data = true;
        }
    }
}
