using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toHideObject : MonoBehaviour {

	public void Hide (GameObject myObject)
	{
		if (myObject.activeSelf)
			myObject.SetActive(false);
		else
			myObject.SetActive(true);
	}
}
