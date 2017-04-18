using System.Collections;
using System.Text.RegularExpressions;
using HoloToolkit.Sharing.SyncModel;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HoloToolkit.Sharing.Spawning
{
	[SyncDataClass]
    public class SyncGraphBar : SyncSpawnedObject //Pour le partage des donnees sur le network
    {
        [SyncData]
        public SyncString GraphName;

        [SyncData]
        public SyncString Ticker;

        [SyncData]
        public SyncFloat Height;

        [SyncData]
        public SyncFloat Width;

        [SyncData]
        public SyncFloat Color_R;

        [SyncData]
        public SyncFloat Color_G;

        [SyncData]
        public SyncFloat Color_B;
    }
}

public class GraphBar : MonoBehaviour {

    public int nbr_bar = 24; //Nombre de bar dans le graph
    private int nbr_bar_save;
    public float height = 1.0f; //hauteur de celui-ci
    public float width = 1.0f; //largeur de celui-ci
    public float time_to_update = 15.0f; //Pour les besoins de la demo = 15 secondes !!! entre 2 RealValues
    public double[] data; //Les fameuses dataprivate
	string[] dataDate;//Les dates associées
	private string RefChangeDate = "hour";
	private GameObject[] bars;
    public GameObject bar_prefab;
    public string graph_name; //Nom du graphique (peut etre modifie a n'importe quel moment dans Unity)
    private Transform CylinderX;
    private Transform CylinderY;
    private Transform Name;
	public string ticker;
	public Color ColorBar = Color.white;
    public SyncGraphBar sync;
    public PrefabSpawnManager SpawnManager;
    public bool online;

	void Restart()
    {
		InitBars();
		UpdateAllGraph();
    }

    public void Destroy_on_Network()
    {
        SpawnManager.Delete(sync);
        Destroy(this.transform.parent);
    }

    IEnumerator Start() //Initialisation..
    {
        online = true;
		SpawnManager = GameObject.Find("Sharing").GetComponent<PrefabSpawnManager>();
        sync = transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncGraphBar;
		ticker = (online ? sync.Ticker.Value : ticker);
		graph_name = (online ? sync.GraphName.Value : graph_name);
		CylinderX = transform.FindChild("CylinderX");
        CylinderY = transform.FindChild("CylinderY");
        Name = transform.FindChild("Name");
		InitBars();
		yield return StartCoroutine("RealValues");
    }

	void InitBars()
	{
		nbr_bar_save = nbr_bar;
		data = new double[nbr_bar];
		if (bars != null)
			for (int i = 0; i < bars.Length; i++)
				Destroy(bars[i]);
		bars = new GameObject[nbr_bar];
		data[0] = ConfigAPI.PriceList[ticker];
		for (int i = 1; i < nbr_bar; i++) //On remplit les donnees avec des nombres fictifs mais cohérents.
			data[i] = data[i - 1] + Random.Range(-1.75f, 1.75f);
		for (int i = 0; i < nbr_bar; i++)
		{
			bars[i] = Instantiate(bar_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
			bars[i].transform.parent = transform;
			bars[i].transform.localScale = new Vector3(width / nbr_bar, 1.0f, width / nbr_bar);
			bars[i].transform.localPosition = new Vector3(i * bars[i].transform.localScale.x, 0.0f, 0.0f);
			bars[i].GetComponent<Bar>().dataDevice = data[i];
			bars[i].GetComponent<Bar>().ColorBar = ColorBar;
		}
		RemplitDatesSelonEchelle(RefChangeDate);
	}

    double MoreDistantData()
    {
        double result = 0.0f;

		foreach (double value in data)
			result = System.Math.Abs(value) > result ? System.Math.Abs(value) : result;
        return (result);
    }

    void InsertData(double d) //Swap les valeurs de Data et insert la nouvelle valeur à la fin
    {
        for (int i = 0; i < nbr_bar - 1; i++)
            data[i] = data[i + 1];
        data[nbr_bar - 1] = d;
    }

    void UpdateAllGraph()
    {
		Update_bar();
		Update_cylinder();
		Update_Collider();
        Put_Name();
    }

    IEnumerator RealValues() //Coroutine pour ajouter regulierement des fausses valeurs au graph
    {
		while (true)
		{
            UnityWebRequest www = UnityWebRequest.Get(ConfigAPI.apiGoogleBasePath + ConfigAPI.getLastPrice + ConfigAPI.paramCompany + ticker);
			yield return www.Send();
			double d = 0.0;
			if (www.downloadHandler.text != "")
				d = parseRequestLastPrices(www.downloadHandler.text);
			else
				d = data[data.Length - 1] + Random.Range(-1.75f, 1.75f);
			InsertData(d < 0.0 ? 0.0 : d);
			UpdateAllGraph();
			yield return new WaitForSeconds(time_to_update);
		}
	}

	private double parseRequestLastPrices (string data)
	{
		string pattern = @"{[^}]+}";
		Match m = Regex.Match(data, pattern); // Regex pour corriger le format du json reçu
		SharePricesM sharePrice;
		sharePrice = JsonUtility.FromJson<SharePricesM>(m.Value);
		return (System.Math.Round(sharePrice.l_fix)); //retour de la valeur intéressante
	}

	void Update_bar() // Met à jour la taille et la position des bars selon leurs nouvelles valeurs
	{
        for (int i = 0; i < nbr_bar; i++)
        {
            bars[i].transform.localScale = new Vector3(bars[i].transform.localScale.x,
                (((float)data[i] / (float)MoreDistantData()) * height) / 2.0f,
				bars[i].transform.localScale.z);
            bars[i].transform.localPosition = new Vector3(bars[i].transform.localPosition.x,
                bars[i].transform.localScale.y, bars[i].transform.localPosition.z);
            bars[i].GetComponent<Bar>().dataDevice = data[i];
			bars[i].GetComponent<Bar>().ColorBar = ColorBar;
		}
		RemplitDatesSelonEchelle(RefChangeDate);
	}

    void Update_cylinder() //Deformation et positionnement des supports cylindriques
    {
        CylinderX.transform.localScale = new Vector3(0.01f, width / 2.0f, 0.01f);
        CylinderX.transform.localPosition = new Vector3(width / 2.0f, 0.0f, 0.0f);
        CylinderY.transform.localScale = new Vector3(0.01f, height / 2.0f, 0.01f);
        CylinderY.transform.localPosition = new Vector3(0.0f, height / 2.0f, 0.0f);
    }

    void Put_Name() //On change le nom du graph et on le scale correctement
    {
        float def = width < height ? width / 10.0f : height / 10.0f;
        Name.transform.localScale = new Vector3(def, def);
        Name.GetComponent<TextMesh>().text = (online ? sync.GraphName.Value : graph_name);
    }

	void Update_Collider () //On scale le box collider à la taille du graphique en cours
    {
        transform.parent.GetComponent<BoxCollider>().center = new Vector3(width / 2.0f, height / 2.0f);
        transform.parent.GetComponent<BoxCollider>().size = new Vector3(width, height);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (online)
            ColorBar = new Color(sync.Color_R.Value, sync.Color_G.Value, sync.Color_B.Value);
        if (nbr_bar_save != nbr_bar)
            Restart();
    }

	public void ChangeNbrPoints (string name)
	{
		switch (name.ToUpper())
		{
			case "1H":
				nbr_bar = 60; time_to_update = 15.0f; RefChangeDate = "minute"; break;
			case "1D":
				nbr_bar = 24; time_to_update = 15.0f; RefChangeDate = "hour"; break;
			case "5D":
				nbr_bar = 24 * 5; time_to_update = 60.0f; RefChangeDate = "hour"; break;
			case "1M":
				nbr_bar = 30; time_to_update = 60.0f; RefChangeDate = "day"; break;
			case "6M":
				nbr_bar = 30 * 6; time_to_update = 60.0f; RefChangeDate = "day"; break;
			case "1Y":
				nbr_bar = 30 * 12; time_to_update = 60.0f; RefChangeDate = "day"; break;
			case "2Y":
				nbr_bar = 24; time_to_update = 60.0f; RefChangeDate = "month"; break;
			case "5Y":
				nbr_bar = 12 * 5; time_to_update = 60.0f; RefChangeDate = "month"; break;
			case "MAX":
				nbr_bar = 60; time_to_update = 60.0f; RefChangeDate = "year"; break;
			default:
				nbr_bar = 70; time_to_update = 60.0f; RefChangeDate = "hour"; break;
		}
		Restart();
	}

	private void RemplitDatesSelonEchelle (string format)
	{
		dataDate = new string[nbr_bar];
		dataDate[nbr_bar - 1] = System.DateTime.Today.ToString("d MMM yyyy HH:mm");
		switch (format.ToLower())
		{
			case "minute":
				for (int i = nbr_bar - 1, i2 = 0; i >= 0; i--, i2++)
				{
					dataDate[i] = System.DateTime.Today.AddMinutes(-i2).ToString("d MMM yyyy HH:mm");
					bars[i].GetComponent<Bar>().dataTime = dataDate[i];
				}
				break;
			case "hour":
				for (int i = nbr_bar - 1, i2 = 0; i >= 0; i--, i2++)
				{
					dataDate[i] = System.DateTime.Today.AddHours(-i2).ToString("d MMM yyyy HH");
					bars[i].GetComponent<Bar>().dataTime = dataDate[i];
				}
				break;
			case "day":
				for (int i = nbr_bar - 1, i2 = 0; i >= 0; i--, i2++)
				{

					dataDate[i] = System.DateTime.Today.AddDays(-i2).ToString("d MMM yyyy");
					bars[i].GetComponent<Bar>().dataTime = dataDate[i];
				}
				break;
			case "month":
				for (int i = nbr_bar - 1, i2 = 0; i >= 0; i--, i2++)
				{

					dataDate[i] = System.DateTime.Today.AddMonths(-i2).ToString("MMM yyyy");
					bars[i].GetComponent<Bar>().dataTime = dataDate[i];
				}
				break;
			case "year":
				for (int i = nbr_bar - 1, i2 = 0; i >= 0; i--, i2++)
				{

					dataDate[i] = System.DateTime.Today.AddYears(-i2).ToString("yyyy");
					bars[i].GetComponent<Bar>().dataTime = dataDate[i];
				}
				break;
		}
	}
}
