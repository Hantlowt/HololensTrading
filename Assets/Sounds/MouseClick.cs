using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour {

	public AudioClip myclip;

	private void Start ()
	{
		this.gameObject.AddComponent<AudioSource>();
		this.GetComponent<AudioSource>().clip = myclip;
	}

	public void OnMouseDown()
	{
		this.GetComponent<AudioSource>().Play();
	}
}
