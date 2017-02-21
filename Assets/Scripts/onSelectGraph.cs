using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class onSelectGraph : MonoBehaviour {

	public GameObject Graph_Prefab;

	/* Au click pour ajouter un graph on récupère le nom de la société et le
	 * ticker correspondant pour lancer l'instanciation du nouveau graph
	 */
	public void onClickOnTypeOfGraph (Text CompanyName)
	{
		print(CompanyName.text);
		int pos = 0;

		while (pos < ConfigAPI.CompanyNameList.Length)
		{
			if (String.Equals(ConfigAPI.CompanyNameList[pos], CompanyName.text))
			{
				DrawNewGraphLine(ConfigAPI.CompanyNameList[pos], ConfigAPI.TickersList[pos]);
				break;
			}
			pos++;
		}
		if (pos == ConfigAPI.CompanyNameList.Length)
			DrawNewGraphLine("UnknowGraph Error", "undefined"); //valeurs d'erreurs, gestion du cas à prévoir/ajouter pour la gestion requete API = TO DO
	}

	/* on instancie avec des valeurs par défaut le nouveau Graph dont le prefab est en paramètre,
	 * on lui donne son bon nom et on lance la mise à jour de ses données selon le ticker en paramètre
	 */
	public void DrawNewGraphLine (string title, string ticker)
	{
		GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.0f, 0.0f, 2.8f), transform.rotation) as GameObject;
		temp.GetComponent<GraphLine>().height = 0.35f;
		temp.GetComponent<GraphLine>().width = 0.8f;
		temp.GetComponent<GraphLine>().graph_name = title;
		temp.GetComponent<GraphLine>().time_to_update = 1.0f;
		temp.GetComponent<MeshRenderer>().material.color = Color.green;

		print("TEST START REQUEST");
		print(ticker);
		getPrices.singleton.StartCoroutine("getPricesDays", ticker);
		print("END TEST");
	}
}