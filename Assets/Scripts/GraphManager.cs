using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {

	public int nbr_graph_to_instantiate = 4;
	public GameObject Graph_Prefab;
	public Color[] colors;
	public string[] titles;
	// Use this for initialization
	void Start () {
		float next_pos = 0.0f;
		float width;
		for (int i = 0; i < nbr_graph_to_instantiate; i++)
		{
			
			Vector3 pos = new Vector3(next_pos, 0.0f, 3.0f);
			GameObject temp = Instantiate(Graph_Prefab, pos, transform.rotation) as GameObject;
			temp.GetComponent<GraphLine>().height = Random.Range(0.25f, 0.45f);
			width = Random.Range(0.3f, 1.0f);
			temp.GetComponent<GraphLine>().width = width;
			temp.GetComponent<GraphLine>().graph_name = titles[i % titles.Length];
			temp.GetComponent<GraphLine>().time_to_update = Random.Range(0.5f, 2.0f);
			temp.GetComponent<MeshRenderer>().material.color = colors[i % colors.Length];
			next_pos += width;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
