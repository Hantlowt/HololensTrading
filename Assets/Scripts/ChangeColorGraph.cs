using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Sharing.SyncModel;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;

public class ChangeColorGraph : MonoBehaviour
{
	public void changeColorGraph (GameObject graph)
	{
        
        if (graph.GetComponent<MeshRenderer>())
		{
            GraphLine g = graph.GetComponent<GraphLine>();
            if (g.online)
            {
                SyncGraphLine sync = graph.transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncGraphLine;
                sync.Color_R.Value = GetComponent<Image>().color.r;
                sync.Color_G.Value = GetComponent<Image>().color.g;
                sync.Color_B.Value = GetComponent<Image>().color.b;
            }
            else
			    graph.GetComponent<MeshRenderer>().material.color = GetComponent<Image>().color;
		}
		else
		{
            GraphBar g = graph.GetComponent<GraphBar>();
            if (g.online)
            {
                SyncGraphBar sync = graph.transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncGraphBar;
                sync.Color_R.Value = GetComponent<Image>().color.r;
                sync.Color_G.Value = GetComponent<Image>().color.g;
                sync.Color_B.Value = GetComponent<Image>().color.b;
            }
            else
                graph.GetComponent<GraphBar>().ColorBar = GetComponent<Image>().color;
		}
	}
}
