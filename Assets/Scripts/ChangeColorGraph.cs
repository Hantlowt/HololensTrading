using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorGraph : MonoBehaviour
{

	public void ChooseColorGraph ()
	{
		onSelectGraph.colorGraph = GetComponent<Image>().color;
	}

	public void changeColorGraph (GameObject graph)
	{
		if (graph.GetComponent<MeshRenderer>())
		{
			graph.GetComponent<MeshRenderer>().material.color = GetComponent<Image>().color;
		}
		else
		{
			graph.GetComponent<GraphBar>().ColorBar = GetComponent<Image>().color;
		}
	}
}
