using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {
    public GameObject[] led_images_prefab;
    private GameObject[] led_images;
    private int pos;
    private int nbr_instance;
    public float speed;
	// Use this for initialization
	void Start () {
        nbr_instance = 4;
        led_images = new GameObject[nbr_instance];
        pos = 0;
        for (int i = 0; i < nbr_instance; i++)
        {
            led_images[i] = Instantiate(led_images_prefab[i % led_images_prefab.Length]);
            led_images[i].transform.parent = this.transform;
            led_images[i].transform.localPosition = new Vector3(4.05f, 0.0f, -0.1f);
        }
    }
	
    void MoveImages()
    {
        for(int i = 0; i < nbr_instance; i++)
        {
            if (led_images[i].transform.localPosition.x > -2.2f)
            {
                if (i > 0)
                {
                    if (led_images[i - 1].transform.localPosition.x <= 1.85f || led_images[i].transform.localPosition.x <= 1.85f)
                        led_images[i].transform.localPosition = new Vector3(led_images[i].transform.localPosition.x - Time.deltaTime * speed,
                        led_images[i].transform.localPosition.y, led_images[i].transform.localPosition.z);
                }
                else
                    led_images[i].transform.localPosition = new Vector3(led_images[i].transform.localPosition.x - Time.deltaTime * speed,
                        led_images[i].transform.localPosition.y, led_images[i].transform.localPosition.z);
            }
            else if (led_images[nbr_instance - 1].transform.localPosition.x <= 1.85f)
                led_images[i].transform.localPosition = new Vector3(4.05f, 0.0f, -0.1f);
        }
    }
	// Update is called once per frame
	void Update () {
        MoveImages();
        /*if (led_images[2] != null &&
            led_images[MathMod(pos - 1, 5)].transform.position.x <= 1.85f)
        {
            led_images[pos] = Instantiate(led_images_prefab[pos % led_images_prefab.Length]);
            led_images[pos].transform.parent = this.transform;
            led_images[pos].transform.position = new Vector3(4.05f, 0.0f, -0.1f);
        }*/
		
	}

    static int MathMod(int a, int b)
    {
        return (System.Math.Abs(a * b) + a) % b;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {
    public GameObject[] led_images_prefab;
    private GameObject[] led_images;
    private int nbr_instance;
    public float speed;
	// Use this for initialization
        led_images = new GameObject[nbr_instance];
        for (int i = 0; i < nbr_instance; i++)
        {
            led_images[i] = Instantiate(led_images_prefab[i % led_images_prefab.Length]);
            led_images[i].transform.parent = this.transform;