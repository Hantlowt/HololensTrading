using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toHideObject : MonoBehaviour {

	public void onClickHide (GameObject myObject)
	{
		if (myObject.activeSelf)
			myObject.SetActive(false);
		else
			myObject.SetActive(true);
	}
}
