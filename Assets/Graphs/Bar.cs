using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
    public double data;
    private GameObject data_text;
    private GameObject Graph;
    public Color normal_color;
    public Color select_color;
	// Use this for initialization
	void Start () {
        data_text = transform.FindChild("data").gameObject;
        data_text.SetActive(false);
        data = 0.0;
        Graph = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10)
            && hit.transform.gameObject == this.gameObject)
        {
            data_text.SetActive(true);
            GetComponent<MeshRenderer>().material.color = select_color;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = normal_color;
            data_text.SetActive(false);
        }
            
        data_text.GetComponent<TextMesh>().text = data.ToString();
        
        float height = Graph.GetComponent<GraphBar>().height;
        float width = Graph.GetComponent<GraphBar>().width;
        float def = (width < height ? width / 10.0f : height / 10.0f);
        //data_text.GetComponent<TextMesh>().fontSize = (int)def;
        data_text.transform.localScale = new Vector3(1.0f / transform.localScale.x * def, 1.0f / transform.localScale.y * def);
        data_text.transform.localPosition = new Vector3(0.0f, 0.5f / transform.localScale.y);
    }
}
