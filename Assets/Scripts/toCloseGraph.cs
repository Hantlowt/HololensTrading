using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toCloseGraph : MonoBehaviour {

	public void onClickDestroy(GameObject parentGraph)
	{
		Destroy(parentGraph, 0.3f);
	}
}
