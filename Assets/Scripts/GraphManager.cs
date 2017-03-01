using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing.Spawning;

public class GraphManager : MonoBehaviour {

	public int nbr_graph_to_instantiate = 4;
	public GameObject Graph_Prefab;
	public Color[] colors;
	public string[] titles;
    public Transform holo;
    public GameObject SpawnParent;
    public PrefabSpawnManager SpawnManager;
    private bool init;
    public bool local;
    // Use this for initialization
    void Start () {
        init = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (!init)
        {
            float next_pos = 0.0f;
            float width;

            for (int i = 0; i < nbr_graph_to_instantiate; i++)
            {

                Vector3 pos = new Vector3(next_pos, 1.5f, -2.5f);
                if (local)
                {
                    GameObject temp = Instantiate(Graph_Prefab, pos, transform.rotation) as GameObject;

                    temp.GetComponent<GraphLine>().height = Random.Range(0.25f, 0.45f);
                    width = Random.Range(0.3f, 1.0f);
                    temp.GetComponent<GraphLine>().width = width;
                    temp.GetComponent<GraphLine>().graph_name = titles[i % titles.Length];
                    temp.GetComponent<GraphLine>().time_to_update = Random.Range(0.5f, 2.0f);
                    temp.GetComponent<MeshRenderer>().material.color = colors[i % colors.Length];
                    temp.transform.SetParent(holo);
                    temp.transform.localPosition = pos;
                    next_pos += width;
                }
                else
                {
                    SyncGraphLine temp = new SyncGraphLine();

                    SpawnManager.Spawn(temp, pos, transform.rotation, SpawnParent, "SyncGraphLine", false);
                    temp.GameObject.GetComponent<GraphLine>().height = Random.Range(0.25f, 0.45f);
                    width = Random.Range(0.3f, 1.0f);
                    temp.GameObject.GetComponent<GraphLine>().width = width;
                    temp.GameObject.GetComponent<GraphLine>().graph_name = titles[i % titles.Length];
                    temp.GameObject.GetComponent<GraphLine>().time_to_update = Random.Range(0.5f, 2.0f);
                    temp.GameObject.GetComponent<MeshRenderer>().material.color = colors[i % colors.Length];
                    temp.GameObject.transform.SetParent(holo);
                    next_pos += width;
                }
            }
            init = true;
        }
	}
}
