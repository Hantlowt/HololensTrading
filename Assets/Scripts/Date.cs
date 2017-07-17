using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Date : MonoBehaviour {

    TextMeshPro textMesh;
	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMeshPro>();
	}
	
	// Update is called once per frame
	void Update () {
        textMesh.text = DateTime.Now.ToString();
	}
}
