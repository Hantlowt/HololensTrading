using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomGui : MonoBehaviour {

	public void zoomIn()
	{
		GetComponent<Transform>().localScale = new Vector3(GetComponent<Transform>().localScale.x * 2, GetComponent<Transform>().localScale.y * 2, GetComponent<Transform>().localScale.z);
	}

	public void zoomOut ()
	{
		GetComponent<Transform>().localScale = new Vector3(GetComponent<Transform>().localScale.x / 2, GetComponent<Transform>().localScale.y / 2, GetComponent<Transform>().localScale.z);
	}
}
