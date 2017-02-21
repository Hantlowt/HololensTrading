using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayCompanyList : MonoBehaviour
{

	private Dropdown dropDown;
	private List<string> options;

	private void Start ()
	{
		//Requête API pour récupérer la liste des entreprises :
		dropDown = GetComponent<Dropdown>();
		dropDown.ClearOptions();

		options = new List<string>();
		foreach (string companyName in ConfigAPI.CompanyNameList)
		{
			options.Add(companyName);
		}
		dropDown.AddOptions(options);
	}
}
