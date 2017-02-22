using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class onSelectGraph : MonoBehaviour {

	public GameObject   Graph_Prefab;
	public static Color colorGraph = Color.blue;

	/* Au click pour ajouter un graph on récupère le nom de la société et le
	 * ticker correspondant pour lancer l'instanciation du nouveau graph
	 */
	public void onClickOnTypeOfGraph (Text CompanyName)
	{
		print(CompanyName.text);
		bool dataForRequestCheck = false;

		foreach (KeyValuePair<string, string> entry in ConfigAPI.CompanyList)
		{
			if (CompanyName.text.Equals(entry.Value))
			{
				drawNewGraphLine(entry.Value, entry.Key);
				dataForRequestCheck = true;
			}
		}
		if (!dataForRequestCheck)
			print("choisir une société dans la liste et rééssayer");
	}

	/* on instancie avec des valeurs par défaut le nouveau Graph dont le prefab est en paramètre,
	 * on lui donne son bon nom et on lance la mise à jour de ses données selon le ticker en paramètre
	 */
	public void drawNewGraphLine (string title, string ticker)
	{
		GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.0f, 0.0f, 2.5f), transform.rotation) as GameObject;
		temp.GetComponent<GraphLine>().height = 0.35f;
		temp.GetComponent<GraphLine>().width = 0.8f;
		temp.GetComponent<GraphLine>().graph_name = title;
		temp.GetComponent<GraphLine>().ticker = ticker;
		temp.GetComponent<GraphLine>().time_to_update = 1.0f;
		temp.GetComponent<MeshRenderer>().material.color = colorGraph;
	}

}