using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
	public GameObject text_image_prefab;
	public GameObject cubeInvisibleLeft;
	public GameObject cubeInvisibleRight;
	private GameObject[] led_image = new GameObject[ConfigAPI.CompanyList.Count];
	Vector3 posStart;
	private int nbr_instance;
	public float speed;

	IEnumerator Start()
    {
		posStart = new Vector3(cubeInvisibleRight.transform.localPosition.x - 0.1f, cubeInvisibleRight.transform.localPosition.y -0.03f, -0.1f);
		nbr_instance = ConfigAPI.CompanyList.Count;
		InitLedImage();
		yield return new WaitForSeconds(1.0f);
	}

	private void InitLedImage ()
	{
		int i = 0;
		foreach (KeyValuePair<string, string> entry in ConfigAPI.CompanyList)
		{
			led_image[i] = Instantiate(text_image_prefab, posStart, Quaternion.identity);
			(led_image[i]).transform.parent = transform;
			led_image[i].transform.localPosition = posStart;
			(led_image[i]).transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			led_image[i].GetComponent<UpdateInfosBanner>().title = entry.Value;
			led_image[i].GetComponent<UpdateInfosBanner>().ticker = entry.Key;
			i++;
		}
	}

	void Update ()
	{
		MoveImages();
	}
	
	private void MoveImages ()
	{
		for (int i = 0; i < nbr_instance; i++)
		{
			if (led_image[i].transform.localPosition.x - 0.1 > cubeInvisibleLeft.transform.localPosition.x)
			{
				if (i > 0 && led_image[i - 1].transform.localPosition.x <= 1.85f)
						led_image[i].transform.localPosition = new Vector3(led_image[i].transform.localPosition.x - Time.deltaTime * speed, led_image[i].transform.localPosition.y, led_image[i].transform.localPosition.z);
				else if (i == 0)
					led_image[i].transform.localPosition = new Vector3(led_image[i].transform.localPosition.x - Time.deltaTime * speed, led_image[i].transform.localPosition.y, led_image[i].transform.localPosition.z);
			}
			else if (led_image[nbr_instance - 1].transform.localPosition.x - 0.1 <= cubeInvisibleLeft.transform.localPosition.x)
				led_image[i].transform.localPosition = posStart;
		}
	}

    static int MathMod(int a, int b)
    {
        return (System.Math.Abs(a * b) + a) % b;
    }
}
