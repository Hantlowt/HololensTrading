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
	public static bool First;
    public ShowOrHide Menu;


	private void Start ()
	{
		First = true;
	}

	private void Update ()
	{
		
	}

	public void drawNewWhiteboard()
    {
		if (First)
		{
			First = false;
			SyncWhiteboard sync = new SyncWhiteboard();
			SpawnManager.Spawn(sync, new Vector3(-0.5f, 0f, 3.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f), transform.parent.transform.parent.transform.parent.gameObject, "SyncWhiteboard", false);
			//ButtonColor.color = Color.grey;
		}
    }

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

    float GetPosSpawn()
    {
        float i = 0.8f;
        GameObject[] graphs = GameObject.FindGameObjectsWithTag("Graph");
        bool ok = false;
        bool exist = false;
        while (!ok)
        {
            foreach (GameObject g in graphs)
                if (g.transform.position == new Vector3(i, 0f, 4.0f))
                    exist = true;
            if (exist)
            {
                i += 0.9f;
                exist = false;
            }
            else
                ok = true;
        }
        return i;
    }

	/* on instancie avec des valeurs par défaut le nouveau Graph dont le prefab est en paramètre,
	 * on lui donne son bon nom et on lance la mise à jour de ses données selon le ticker en paramètre
	 */
	public void drawNewGraphLine (Text CompanyName)
	{
        Menu.Hide();
		FindTitleAndTicker(CompanyName);
		GameObject graphChild;
        if (!online)
        {
            GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.8f, 0f, 4.0f), transform.rotation, transform.parent.transform.parent.transform.parent) as GameObject;
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
            SpawnManager.Spawn(sync, new Vector3(GetPosSpawn(), 0f, 4.0f), transform.rotation, transform.parent.transform.parent.transform.parent.gameObject, "SyncGraphLine", false);
        }
    }

	public void drawNewGraphBar (Text CompanyName)
	{
        Menu.Hide();
		FindTitleAndTicker(CompanyName);
		if (!online)
        {
            GameObject temp = Instantiate(Graph_Prefab, new Vector3(0.8f, 0f, 4.0f), transform.rotation, transform.parent.transform.parent.transform.parent) as GameObject;
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
            SpawnManager.Spawn(sync, new Vector3(GetPosSpawn(), 0f, 4.0f), transform.rotation, transform.parent.transform.parent.transform.parent.gameObject, "SyncGraphBar", false);
        }
	}

}