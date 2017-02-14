using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class Selector : MonoBehaviour, IInputClickHandler
{
    public GameObject selected_object;
    public float distance_selected;
    public Quaternion lockrot;

	// Use this for initialization
	void Start () {
        selected_object = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (selected_object != null)
        {
            //Vector3 new_pos;
            selected_object.transform.LookAt(2 * transform.position - Camera.main.transform.position);
            selected_object.transform.localRotation = Quaternion.identity;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10)
                && hit.transform.gameObject.tag != "Graph")
                selected_object.transform.position = hit.transform.position;
            else
                selected_object.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance_selected;
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10))
        {
            if (selected_object == null && hit.transform.gameObject.tag == "Graph")
            {
                selected_object = hit.transform.gameObject;
                selected_object.transform.parent = Camera.main.transform;
                distance_selected = Vector3.Distance(transform.position, Camera.main.transform.position);
                lockrot = selected_object.transform.rotation;
                selected_object.GetComponent<BoxCollider>().enabled = false;

            }
            else if(selected_object != null)
            {
                selected_object.transform.parent = null;
                selected_object.GetComponent<BoxCollider>().enabled = true;
                selected_object = null;
            }
            //selected_object.transform.localPosition = GetComponent<BoxCollider>().center;
            
        }
    }
}
