using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class onSelectGraph : MonoBehaviour {

	public GameObject   Graph_Prefab;
	public static Color colorGraph = Color.blue;
	private static string graphTicker = "GOOG"; //tmp test graph si pas de société choisit
	private string graphTitle = ConfigAPI.CompanyList[graphTicker]; //tmp test graph si pas de société choisit

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
				graphTitle = entry.Value;
				graphTicker = entry.Key;
				dataForRequestCheck = true;
			}
		}
		if (!dataForRequestCheck)
			print("choisir une société dans la liste et rééssayer");
	}

	/* on instancie avec des valeurs par défaut le nouveau Graph dont le prefab est en paramètre,
	 * on lui donne son bon nom et on lance la mise à jour de ses données selon le ticker en paramètre
	 */
	public void drawNewGraphLine ()
	{
		GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.8f, 0.25f, 2.9f), transform.rotation) as GameObject;
		temp.GetComponent<GraphLine>().height = 0.35f;
		temp.GetComponent<GraphLine>().width = 0.8f;
		temp.GetComponent<GraphLine>().graph_name = graphTitle;
		temp.GetComponent<GraphLine>().ticker = graphTicker;
		temp.GetComponent<GraphLine>().time_to_update = 1.0f;
		temp.GetComponent<MeshRenderer>().material.color = colorGraph;
	}

	public void drawNewGraphBar ()
	{
		GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.8f, 0.25f, 2.9f), transform.rotation) as GameObject;
		temp.GetComponent<GraphBar>().graph_name = graphTitle;
		temp.GetComponent<GraphBar>().ticker = graphTicker;
		temp.GetComponent<MeshRenderer>().material.color = colorGraph;
	}

}