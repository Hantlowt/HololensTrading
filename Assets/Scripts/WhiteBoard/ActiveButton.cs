using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveButton : MonoBehaviour {
	public GameObject[] ButtonToDisable;

	private void start ()
	{
		GetComponent<Image>().color = Color.grey;
		for (int i = 0; i < ButtonToDisable.Length; i++)
			ButtonToDisable[i].GetComponent<Image>().color = Color.grey;
	}

	public void ModeIsActive ()
	{
		Color tmp = GetComponent<Image>().color;
		if (tmp == Color.green)
			GetComponent<Image>().color = Color.grey;
		else
		{
			GetComponent<Image>().color = Color.green;
			for (int i = 0; i < ButtonToDisable.Length; i++)
				ButtonToDisable[i].GetComponent<Image>().color = Color.grey;
		}
	}

	public void ActiveButtonJustOneSecond ()
	{
		GetComponent<Image>().color = Color.green;
		new WaitForSeconds(2.0f);
		GetComponent<Image>().color = Color.white;
	}

	public void ActiveButtonOnDown ()
	{
		GetComponent<Image>().color = Color.green;
		for (int i = 0; i < ButtonToDisable.Length; i++)
			ButtonToDisable[i].GetComponent<Image>().color = Color.grey;
	}

	public void DesActiveButtonOnUp ()
	{
		GetComponent<Image>().color = Color.white;
	}
}
