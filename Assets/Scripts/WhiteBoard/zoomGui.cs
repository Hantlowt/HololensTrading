using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomGui : MonoBehaviour {

	private float ScaleToRef;

	private void Start()
	{
		ScaleToRef = GetComponent<Transform>().localScale.x;
	}

	public void zoomIn()
	{
		if (ScaleToRef == GetComponent<Transform>().localScale.x)
		{
			GetComponent<Transform>().localScale = new Vector3(GetComponent<Transform>().localScale.x * 2, GetComponent<Transform>().localScale.y * 2, GetComponent<Transform>().localScale.z);
		}
	}

	public void zoomOut ()
	{
		if (ScaleToRef < GetComponent<Transform>().localScale.x)
		{
			GetComponent<Transform>().localScale = new Vector3(GetComponent<Transform>().localScale.x / 2, GetComponent<Transform>().localScale.y / 2, GetComponent<Transform>().localScale.z);
		}
	}
}
