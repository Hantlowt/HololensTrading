using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawNewGraph : MonoBehaviour {

	public GameObject Graph_Prefab;

	public void DrawNewGraphLine (string title)
	{
		GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.0f, 0.0f, 3.0f), transform.rotation) as GameObject;
		temp.GetComponent<GraphLine>().height = 0.35f;
		temp.GetComponent<GraphLine>().width = 0.8f;
		temp.GetComponent<GraphLine>().graph_name = title;
		temp.GetComponent<GraphLine>().time_to_update = 1.0f;
		temp.GetComponent<MeshRenderer>().material.color = Color.green;
	}
}
