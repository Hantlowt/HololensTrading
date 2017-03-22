using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing.SyncModel;
using HoloToolkit.Sharing;

public class onSelectGraph : MonoBehaviour {

	public GameObject   Graph_Prefab;
	public static Color colorGraph = Color.blue;
	private static string graphTicker = "GLE:EPA"; //tmp test graph si pas de société choisit
	private string graphTitle = "Société Générale"; //tmp test graph si pas de société choisit
    public bool online;
    public PrefabSpawnManager SpawnManager;
	/* Au click pour ajouter un graph on récupère le nom de la société et le
	 * ticker correspondant pour lancer l'instanciation du nouveau graph
	 */

    private void FindTitleAndTicker (Text CompanyName)
	{
		foreach (KeyValuePair<string, string> entry in ConfigAPI.CompanyList)
		{
			if (CompanyName.text.Equals(entry.Value))
			{
				graphTitle = entry.Value;
				graphTicker = entry.Key;
			}
		}
	}

	/* on instancie avec des valeurs par défaut le nouveau Graph dont le prefab est en paramètre,
	 * on lui donne son bon nom et on lance la mise à jour de ses données selon le ticker en paramètre
	 */
	public void drawNewGraphLine (Text CompanyName)
	{
		FindTitleAndTicker(CompanyName);
		GameObject graphChild;
        if (!online)
        {
            GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.8f, 0.25f, 4.0f), transform.rotation, transform.parent.transform.parent.transform.parent) as GameObject;
            graphChild = temp.transform.GetChild(0).gameObject;
            graphChild.GetComponent<GraphLine>().online = false;
            graphChild.GetComponent<GraphLine>().height = 0.35f;
            graphChild.GetComponent<GraphLine>().width = 0.8f;
			graphChild.GetComponent<GraphLine>().graph_name = graphTitle;
            graphChild.GetComponent<GraphLine>().ticker = graphTicker;
            graphChild.GetComponent<GraphLine>().time_to_update = 1.0f;
            graphChild.GetComponent<MeshRenderer>().material.color = colorGraph;
        }
        else
        {
            SyncGraphLine sync = new SyncGraphLine();
            sync.Height.Value = 0.35f;
            sync.Width.Value = 0.8f;
			sync.GraphName.Value = graphTitle;
            sync.Ticker.Value = graphTicker;
            sync.Color_R.Value = 1.0f;
            sync.Color_G.Value = 0.0f;
            sync.Color_B.Value = 0.0f;
            SpawnManager.Spawn(sync, new Vector3(0.8f, 0.25f, 4.0f), transform.rotation, transform.parent.transform.parent.transform.parent.gameObject, "SyncGraphLine", false);
        }
    }

	public void drawNewGraphBar (Text CompanyName)
	{
		FindTitleAndTicker(CompanyName);
		if (!online)
        {
            GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.8f, 0.25f, 4.0f), transform.rotation, transform.parent.transform.parent.transform.parent) as GameObject;
            GameObject graphChild = temp.transform.GetChild(0).gameObject;
			graphChild.GetComponent<GraphBar>().graph_name = graphTitle;
            graphChild.GetComponent<GraphBar>().ticker = graphTicker;
			graphChild.GetComponent<GraphBar>().ColorBar = colorGraph;
		}
        else
        {
			SyncGraphBar sync = new SyncGraphBar();
            sync.GraphName.Value = graphTitle;
            sync.Ticker.Value = graphTicker;
            sync.Color_R.Value = 1.0f;
            sync.Color_G.Value = 0.0f;
            sync.Color_B.Value = 0.0f;
            SpawnManager.Spawn(sync, new Vector3(0.8f, 0.25f, 4.0f), transform.rotation, transform.parent.transform.parent.transform.parent.gameObject, "SyncGraphBar", false);
        }
	}

}