using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing.SyncModel;
using HoloToolkit.Sharing;
using HoloToolkit.Unity;

public class onSelectGraph : MonoBehaviour {

	public GameObject   Graph_Prefab;
	public static Color colorGraph = Color.blue;
	private static string graphTicker = "GLE:EPA"; //tmp test graph si pas de société choisit
	private string graphTitle = "Société Générale"; //tmp test graph si pas de société choisit
    public PrefabSpawnManager SpawnManager;
    public static bool whiteboardSpawned = false;
    public ShowOrHide Menu;
    public Button linkedButton;


	private void Start ()
	{
		
	}

	private void Update ()
	{
		
	}

	public void drawNewWhiteboard()
    {
		if (!whiteboardSpawned)
		{
            Menu.Hide();
            whiteboardSpawned = false;
			SyncWhiteboard sync = new SyncWhiteboard();
			SpawnManager.Spawn(sync, new Vector3(0f, 0f, 3.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f), transform.parent.transform.parent.transform.parent.gameObject, "SyncWhiteboard", false);
            linkedButton.interactable = false;
            SpawnManager.lastSpawn.GetComponent<SimpleTagalong>().enabled = true;
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

	/* on instancie avec des valeurs par défaut le nouveau Graph dont le prefab est en paramètre,
	 * on lui donne son bon nom et on lance la mise à jour de ses données selon le ticker en paramètre
	 */
	public void drawNewGraphLine (Text CompanyName)
	{
        Menu.Hide();
		FindTitleAndTicker(CompanyName);
		GameObject graphChild;
        SyncGraphLine sync = new SyncGraphLine();
        sync.Height.Value = 0.35f;
        sync.Width.Value = 0.8f;
		sync.GraphName.Value = graphTitle;
        sync.Ticker.Value = graphTicker;
        sync.Color_R.Value = 1.0f;
        sync.Color_G.Value = 0.0f;
        sync.Color_B.Value = 0.0f;
        SpawnManager.Spawn(sync, new Vector3(0f, 0f, 4.0f), transform.rotation,
            transform.parent.transform.parent.transform.parent.gameObject, "SyncGraphLine", false);
        SpawnManager.lastSpawn.GetComponent<SimpleTagalong>().enabled = true;
    }

	public void drawNewGraphBar (Text CompanyName)
	{
        Menu.Hide();
		FindTitleAndTicker(CompanyName);
		SyncGraphBar sync = new SyncGraphBar();
        sync.GraphName.Value = graphTitle;
        sync.Ticker.Value = graphTicker;
        sync.Color_R.Value = 1.0f;
        sync.Color_G.Value = 0.0f;
        sync.Color_B.Value = 0.0f;
        SpawnManager.Spawn(sync, new Vector3(0f, 0f, 4.0f), transform.rotation,
            transform.parent.transform.parent.transform.parent.gameObject, "SyncGraphBar", false);
        SpawnManager.lastSpawn.GetComponent<SimpleTagalong>().enabled = true;
    }

}