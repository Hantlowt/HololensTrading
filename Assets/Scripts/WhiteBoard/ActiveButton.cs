using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveButton : MonoBehaviour {
	public GameObject ButtonToDisable;

	private void start ()
	{
		GetComponent<Image>().color = Color.grey;
		ButtonToDisable.GetComponent<Image>().color = Color.grey;
	}

	public void ModeIsActive ()
	{
		Color tmp = GetComponent<Image>().color;
		if (tmp == Color.green)
			GetComponent<Image>().color = Color.grey;
		else
			GetComponent<Image>().color = Color.green;
		ButtonToDisable.GetComponent<Image>().color = Color.grey;
	}
}
