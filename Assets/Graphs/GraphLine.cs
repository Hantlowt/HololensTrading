using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

/* 
 * La classe GraphLine pour pouvoir gerer les Graphiques en Lignes.
 * Le but etant de les rendre jolie mais aussi dynamique :)
 */
public class GraphLine : MonoBehaviour
{

    public int nbr_points = 50; //Nombre de points dans le graph
	private int nbr_points_save;
	public float height = 1.0f; //hauteur de celui-ci
    public float width = 1.0f; //largeur de celui-ci
	public float time_to_update = 1f; //Pour les besoins de la demo, temps qui s'ecoulent avant l'add de data
    public double[] data; //Les fameuses data
	public string graph_name; //Nom du graphique, peut-etre modifie a n'importe quel moment
	public string ticker; // abrégé du nom de société pour la requete API
    private LineRenderer linerender;
	private Vector2[] vertices2d;
	private bool data_selected = false; //Lorsque l'on regarde le graph, les donnees s'affichent.. ou non
	private Transform CylinderX;
	private Transform CylinderY;
	private Transform Name;
	private Transform Selected_Data;
    private bool raycast;
    private RaycastHit hit;
	public static string[][] dataPrices; //données requete API des 25 derniers jours
	private static SharePricesM sharePriceList;

	void Restart()
	{
		StopAllCoroutines();
		Start();
	}

    IEnumerator Start() //Initialisation..
    {
		Selected_Data = transform.FindChild("Selected_Data");
		CylinderX = transform.FindChild("CylinderX");
		CylinderY = transform.FindChild("CylinderY");
		Name = transform.FindChild("Name");
		nbr_points_save = nbr_points;
        linerender = GetComponent<LineRenderer>();
		linerender.numPositions = nbr_points;
        data = new double[nbr_points];

		//////////////////////////////////////////////////////////////////////////
		// !! MEMO DD TO DO => A REMPLIR AVEC DES DONNEES COHERENTE REQUEST API //
		//////////////////////////////////////////////////////////////////////////

		UnityWebRequest www = UnityWebRequest.Get(ConfigAPI.apiGoogleBasePath + ConfigAPI.getLastPrice + ConfigAPI.paramCompany + ticker);
		yield return www.Send();
		print("URL" + www.url);
		print("resultat brut du lastPrice : " + www.downloadHandler.text);
		data[0] = parseRequestLastPrices(www.downloadHandler.text);
		for (int i = 1; i < nbr_points; i++)
			data[i] = data[i - 1] + Random.Range(-0.5f, 0.5f);
		/* version d'Antoine - sauvegarde en attendant fonctionnement API :
		 *
		data[0] = 5.0f;
		for (int i = 1; i < nbr_points; i++) //On remplit les donnees avec n'importe quoi
			data[i] = data[i - 1] + Random.Range(-0.5f, 0.5f);
		*/
		vertices2d = new Vector2[nbr_points + 2];
        raycast = false;
		Update_Name();
		//StartCoroutine("getPricesDays", ticker);
		StartCoroutine("FakeValues");
	}

	public IEnumerator getPricesDays (string ticker)
	{
		print("in getprice : " + ticker);

		UnityWebRequest www = UnityWebRequest.Get(ConfigAPI.apiGoogleBasePath + ConfigAPI.getPrice + ConfigAPI.paramCompany + ticker);
		yield return www.Send();
		print("URL" + www.url);
		print("resultat brut requete 25 derniers jours : " + www.downloadHandler.text);
		dataPrices = parseRequestAllPrices(www.downloadHandler.text);
	}

	/* pour info retour requete des 25 derniers jours :
	EXCHANGE%3DNASDAQ
MARKET_OPEN_MINUTE=570
MARKET_CLOSE_MINUTE=960
INTERVAL=86400
COLUMNS=DATE,CLOSE,HIGH,LOW,OPEN,VOLUME
DATA=
TIMEZONE_OFFSET=-300
a1485205200,42.4,42.45,41.84,41.95,13662837
1,43.9,44.22,43.43,43.65,24933488
2,44.94,45,44.09,44.11,14388287
3,44.55,45.08,44.4,44.83,9974613
4,44.42,44.71,44.23,44.54,4943138
7,43.93,44.22,43.705,44.22,6906121
8,44.07,44.15,43.67,43.78,5153697
9,43.78,44.26,43.71,44.25,5307216
10,43.69,43.995,43.58,43.63,5065117
11,43.71,43.99,43.66,43.9,6422438
14,44.42,44.53,43.56,43.62,7308775
15,44.37,44.5934,44.19,44.43,4445846
16,45.07,45.19,44.34,44.5,6531000
17,45.08,45.24,44.82,44.95,3769280
18,45.03,45.23,44.96,45.15,4158744
21,45.46,45.57,44.94,45.08,7491311
22,45.02,45.58,44.9,45.38,6676842
23,45.65,46.72,44.64,45.03,22509206
24,45.16,45.69,45.01,45.65,8972022
25,45.1,45.34,44.79,44.95,9417191
		*/

	/*
	 * récupère les données de la requete et les retournent sous forme de tableau, de tableau de string
	 * supprime les 8 premières lignes dont on n'a pas besoin pour ne conserver que les dates et prix
	 * newData[i] contient alors: DATE,CLOSE,HIGH,LOW,OPEN,VOLUME
	 */
	private string[][] parseRequestAllPrices (string data)
	{
		string[] stringTmp = data.Split('\n');
		string[][] newData = new string[stringTmp.Length - 8][];
		int i = 0; //index pour remplir le nouveau tableau
		int i2 = 0; //index pour supprimer les lignes inutiles
		foreach (string substring in stringTmp)
		{
			if (i2 < 8)
				i2++;
			else
				newData[i++] = substring.Split(',');
		}
		//printDataRequestDebug(newData);
		return (newData);
	}
	private void printDataRequestDebug (string[][] data)
	{
		foreach (string[] val1 in data)
		{
			foreach (string val2 in val1)
			{
				print(val2);
			}
		}
	}
	double MoreDistantData()
    {
        double result = 0.0f;
        for (int i = 0; i < nbr_points; i++)
            result = (System.Math.Abs(data[i]) > result ? System.Math.Abs(data[i]) : result);
        return (result);
    }

    void InsertData(double d)
    {
		if (d > data[nbr_points - 1])
			linerender.endColor = Color.green; //Des variations de couleurs si les chiffres sont en hausse..
		else
			linerender.endColor = Color.red; //.. ou en baisse :)
		for (int i = 0; i < nbr_points - 1; i++)
            data[i] = data[i + 1];
        data[nbr_points - 1] = d;
    }

	void Update_All()
	{
		Update_points_position();
		Update_cylinder();
		Update_Collider();
	}

	
	IEnumerator FakeValues () //Coroutine pour ajouter regulierement des valeurs au graph par requete API
	{
		while (true)
		{
			UnityWebRequest www = UnityWebRequest.Get(ConfigAPI.apiGoogleBasePath + ConfigAPI.getLastPrice + ConfigAPI.paramCompany + ticker);
			yield return www.Send();
			print("URL" + www.url);
			print("resultat brut du lastPrice : " + www.downloadHandler.text);
		//sharePriceList = JsonUtility.FromJson<SharePricesM>(jsonCorrige);
		double d = parseRequestLastPrices(www.downloadHandler.text);
		print("new price = " + d);
			//double d = data[nbr_points - 1] + (double)Random.Range(-0.5f, 0.5f); d = (d > 10.0 ? 10.0 : d);
			InsertData((d < 0.0 ? 0.0 : d));
			Update_All(); //Et remettre a jour le graph
			yield return new WaitForSeconds(time_to_update);
		}
	}

	private double parseRequestLastPrices (string data)
	{
		string tmp = null;
		string pattern = @"""l"" : ""\d+.\d+""";
		foreach (Match m in Regex.Matches(data, pattern))
		{
			string pattern2 = @"\d+.\d+";
			foreach (Match m2 in Regex.Matches(m.Value, pattern2))
			{
				tmp = m2.Value;
			}
		}
		/*
		print("data = " + data);
		string searchStart = "\"l\" : \"";
		string searchEnd = "\" ,\"l_fix";
		int indexStart = data.IndexOf(searchStart);
		print("start : " + indexStart);
		int indexLength = data.IndexOf(searchEnd) - indexStart;
		print("length : " + indexLength);
		string tmp = data.Substring(indexStart, indexLength);*/
		print("tmp : " + tmp);
		return (System.Convert.ToDouble(tmp));
	}


		/* version d'Antoine - sauvegarde en attendant fonctionnement API :
		 *
		IEnumerator FakeValues() //Coroutine pour ajouter regulierement des fausses valeurs au graph
		{
			while (true)
			{
				double d = data[nbr_points - 1] + (double)Random.Range(-0.5f, 0.5f);
				d = (d > 10.0 ? 10.0 : d);
				InsertData((d < 0.0 ? 0.0 : d));
				Update_All(); //Et remettre a jour le graph
				yield return new WaitForSeconds(time_to_update);
			}
		}
		*/

		/* 
		 * Permets de mettre à jour la position des points dans le monde
		 * et genere un mesh a la volee afin de "dessiner" le support d'en dessous des points de la ligne
		 */
		void Update_points_position()
    {
		vertices2d[0] = new Vector2(0.0f, -0.001f);
		for (int i = 0; i < nbr_points; i++) //Pour chaque points...
        {
            Vector3 newPos = new Vector3();
            newPos.x = (width / nbr_points) * i;
            if (MoreDistantData() != 0.0)
                newPos.y = (((float)data[i] / (float)MoreDistantData()) * height); //on calcule la position...
            newPos.z = 0.0f;
			vertices2d[i + 1] = newPos; //et on genere les vertices
			if (i == nbr_points - 1)
			{
				vertices2d[nbr_points + 1] = newPos;
				vertices2d[nbr_points + 1].y = -0.001f; //Meme chose ici pour le dernier point.
			}
            linerender.SetPosition(i, newPos);
        }
		Mesh_Generator m = new Mesh_Generator(vertices2d); //On genere le mesh !
		m.Apply_Mesh(this.gameObject);
	}

	void Update_cylinder() //Deformation et positionnement des supports cylindriques
	{
		CylinderX.transform.localScale = new Vector3(0.005f, width / 2.0f, 0.005f);
		CylinderX.transform.localPosition = new Vector3(width / 2.0f, 0.0f, 0.0f);
		CylinderY.transform.localScale = new Vector3(0.005f, height / 2.0f, 0.005f);
		CylinderY.transform.localPosition = new Vector3(0.0f, height / 2.0f, 0.0f);
	}
	
	void Update_Name() //On change le nom du graph et on le scale correctement
	{
		float def = (width < height ? width / 10.0f : height / 10.0f);
		Name.transform.localScale = new Vector3(def, def);
		Name.GetComponent<TextMesh>().text = graph_name;
	}

	void Update_Collider() //On scale le box collider, important pour le raycast en dessous..
	{
		GetComponent<BoxCollider>().center = new Vector3(width / 2.0f, height / 2.0f);
		GetComponent<BoxCollider>().size = new Vector3(width, height);
	}

	/* Recupere l'id d'un point du graph le plus proche d'une position dans l'espace
	 * Utile pour l'affichage des donnes du graph en fonction de la ou l'on regarde, un peu plus bas */
	int Nearest_Points (Vector3 pos)
	{
		float dist = Mathf.Infinity;
		int result_id = 0;
		pos.y = 0.0f;
		for (int i = 0; i < nbr_points; i++)
		{
			Vector3 pos_point = transform.TransformPoint(linerender.GetPosition(i));
			pos_point.y = pos.y;
			float temp = Vector3.Distance(pos, pos_point);
			if (temp < dist)
			{
				dist = temp;
				result_id = i;
			}
		}
		return result_id;
	}

	// Update is called once per frame
	/*void Update()
    {
		if (nbr_points_save != nbr_points)
			Restart();
		if (raycast && hit.transform.gameObject == this.gameObject)
			data_selected = true;
		else
			data_selected = false;
		Update_Selected_Data(hit.point);
        raycast = false;
	}*/
	
	/* Alors... Cette fonction de barbare permet de faire un truc super classe :
	 * Afficher les donnees du graph !
	 * Bon en gros, si le curseur est passe sur le graph, data_selected est vrai
	 * Ainsi, on affiche le gameobject child de la data selected qui est composee de :
	 * - la valeur que l'on observe (il faut donc modifier le texte);
	 * - ainsi que d'un trait indicateur, realise par un line_renderer. Il faut donc changer le position de ce trait avec la position du texte,
	 * et mettre à jour la taille du tout selon l'echelle du graph.
	 * La fonction Nearest_Point permet de recuperer l'id d'un point du graph dont une position
	 * (celle du curseur en l'occurrence) est la plus proche. C'est ce qui permet de savoir
	 * quelle donnees afficher.
	 */ 
	void Update_Selected_Data(Vector3 pos)
	{
		if (data_selected)
		{
			Selected_Data.gameObject.SetActive(true);
			int id = Nearest_Points(pos);
			float def = width < height ? width / 10.0f : height / 10.0f;
			Selected_Data.GetComponent<TextMesh>().text = System.Math.Round(data[id], 3).ToString();
			Vector3 pos_point_line = linerender.GetPosition(id);
			pos_point_line.y += 0.1f;
			Selected_Data.localPosition = pos_point_line;
			Selected_Data.localScale = new Vector3(def, def);
			pos_point_line = new Vector3(0.0f, -1.5f, -0.3f);
			Selected_Data.GetComponent<LineRenderer>().SetPosition(0, pos_point_line);
			pos_point_line.y -= linerender.GetPosition(id).y / Selected_Data.localScale.y + 1.5f;
			Selected_Data.GetComponent<LineRenderer>().SetPosition(1, pos_point_line);
		}
		else
			Selected_Data.gameObject.SetActive(false);
	}

    void Raycast_Receiver(RaycastHit h)
    {
        raycast = true;
        hit = h;
    }
}
