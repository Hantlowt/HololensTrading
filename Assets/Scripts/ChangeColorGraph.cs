using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorGraph : MonoBehaviour {

	public void ChooseColorGraph ()
	{
		print(GetComponent<Image>().color);
		onSelectGraph.colorGraph = GetComponent<Image>().color;
	}
}
