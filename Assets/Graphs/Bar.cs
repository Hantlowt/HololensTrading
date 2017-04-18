using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
    public double dataDevice;
	public string dataTime;
	private GameObject data_text_Time;
	private GameObject data_text_Device;
	private GameObject Graph;
    public bool raycast;
    public RaycastHit hit;
	public Color ColorBar = Color.white;
	public Transform Selected_Data_Device = null;

	// Use this for initialization
	void Start () {
		Graph = transform.parent.gameObject;
		raycast = false;

		data_text_Time = transform.FindChild("dataTime").gameObject;
        data_text_Time.SetActive(false);
		dataTime = "";

		//Selected_Data_Device = transform.FindChild("Selected_Data_Device");
		//Selected_Data_Device.gameObject.SetActive(false);
		data_text_Device = transform.FindChild("dataDevice").gameObject;
		data_text_Device.SetActive(false);
		dataDevice = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (raycast)
		{
			GetComponent<MeshRenderer>().material.color = Color.white;
			data_text_Time.SetActive(true);
			data_text_Device.SetActive(true);
		}
        else
        {
            GetComponent<MeshRenderer>().material.color = ColorBar;
			data_text_Time.SetActive(false);
			data_text_Device.SetActive(false);
		}

		data_text_Time.GetComponent<TextMesh>().text = dataTime;
		data_text_Device.GetComponent<TextMesh>().text = Math.Round(dataDevice, 3).ToString();

		float height = Graph.GetComponent<GraphBar>().height;
        float width = Graph.GetComponent<GraphBar>().width;
        float def = (width < height ? width / 10.0f : height / 10.0f);
        data_text_Time.transform.localScale = new Vector3(1.0f / transform.localScale.x * def, 1.0f / transform.localScale.y * def);
        data_text_Time.transform.localPosition = new Vector3(0.0f, -1.35f, 0.0f);
		data_text_Device.transform.localScale = new Vector3(1.0f / transform.localScale.x * def, 1.0f / transform.localScale.y * def);
		data_text_Device.transform.localPosition = new Vector3(1.0f, 0.1f, 0.0f);

		/*
		 * Selected_Data_Device.GetComponent<TextMesh>().text = Math.Round(dataDevice, 3).ToString();
		Selected_Data_Device.localScale = new Vector3(1.0f / transform.localScale.x * def, 1.0f / transform.localScale.y * def);
		//!!!!!!!!!!!!!!!!!!!!!!
		//POSITION NAZE ... :/ :/ A CORRIGER :
		Selected_Data_Device.localPosition = new Vector3(-2.0f * transform.localScale.x * def, 0.0f, -2f);
		Selected_Data_Device.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0.0f, height * 2.0f, 0.0f));
		Selected_Data_Device.GetComponent<LineRenderer>().SetPosition(1, new Vector3(23.5f, height * 2.0f, 0.0f));
		//!!!!!!!!!!!!!!!!!!!!!!!!!!
		*/

		raycast = false;
    }

    void Raycast_Receiver(RaycastHit h)
    {
        raycast = true;
        hit = h;
    }
}
