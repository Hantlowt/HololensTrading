using UnityEngine;
using System.Collections;
using HoloToolkit.Sharing.SyncModel;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;
using UnityEngine.Networking;
using System.Text.RegularExpressions;


namespace HoloToolkit.Sharing.Spawning
{
    [SyncDataClass]
    public class SyncGraphLine : SyncSpawnedObject //Pour le partage des donnees sur le network
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

/* 
 * La classe GraphLine pour pouvoir gerer les Graphiques en Lignes.
 * Le but etant de les rendre jolie mais aussi dynamique :)
 */
public class GraphLine : MonoBehaviour
{

    public int nbr_points = 60; //Nombre de points dans le graph
	private int nbr_points_save;
	public float height = 1.0f; //hauteur de celui-ci
    public float width = 1.0f; //largeur de celui-ci
	public float time_to_update = 15.0f; //Une update de la google API a lieue toutes les 15 secondes environs
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
    public SyncGraphLine sync;
    public bool online;


    public void Restart()
	{
		StopAllCoroutines();
		Start();
	}

	IEnumerator Start() //Initialisation..
    {
        online = true;
        sync = transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncGraphLine;
        Selected_Data = transform.FindChild("Selected_Data");
		CylinderX = transform.FindChild("CylinderX");
		CylinderY = transform.FindChild("CylinderY");
		Name = transform.FindChild("Name");
		nbr_points_save = nbr_points;
        linerender = GetComponent<LineRenderer>();
        linerender.enabled = false;
        linerender.numPositions = nbr_points;
        data = new double[nbr_points];
		data[0] = 50.0f;
		for (int i = 1; i < nbr_points; i++)
			data[i] = data[i - 1] + Random.Range(-2.5f, 2.5f);
		
		vertices2d = new Vector2[nbr_points + 2];
        raycast = false;
		//yield return StartCoroutine("getPricesDays", ticker);
		yield return StartCoroutine("RealValues");
	}

	private void Put_Name () //On change le nom du graph et on le scale correctement
	{
		float def = (width < height ? width / 10.0f : height / 10.0f);
		Name.transform.localScale = new Vector3(def, def);
		Name.GetComponent<TextMesh>().text = (online ? sync.GraphName.Value : graph_name);
	}

	private void UpdateAllGraph ()
	{
		Update_points_position();
		Update_cylinder();
		Update_Collider();
        Put_Name();
    }

	IEnumerator RealValues () //Coroutine pour ajouter regulierement des fausses valeurs au graph
	{
		while (true)
		{
            ticker = (online ? sync.Ticker.Value : ticker);
			UnityWebRequest www = UnityWebRequest.Get(ConfigAPI.apiGoogleBasePath + ConfigAPI.getLastPrice + ConfigAPI.paramCompany + ticker);
			yield return www.Send();
            double d = 0.0;
            if (www.downloadHandler.text != "")
                d = parseRequestLastPrices(www.downloadHandler.text);
            else
                d = data[data.Length - 1] + Random.Range(-0.50f, 0.50f);
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
		sharePrice = JsonUtility.FromJson<SharePricesM>(m.Value); //enregistrement des données du json dans un objet SharePriceM
		return (sharePrice.l_fix); //retour de la valeur intéressante
	}

	private void InsertData (double d)
	{
		if (d > data[nbr_points - 1])
			linerender.endColor = Color.green; //Des variations de couleurs si les chiffres sont en hausse..
		else
			linerender.endColor = Color.red; //.. ou en baisse :)
		for (int i = 0; i < nbr_points - 1; i++)
			data[i] = data[i + 1];
		data[nbr_points - 1] = d;
	}

	double MoreDistantData()
    {
        double result = 0.0f;
        for (int i = 0; i < nbr_points; i++)
            result = (System.Math.Abs(data[i]) > result ? System.Math.Abs(data[i]) : result);
        return (result);
    }

		/* 
		 * Permets de mettre à jour la position des points dans le monde
		 * et genere un mesh a la volee afin de "dessiner" le support d'en dessous des points de la ligne
		 */
	void Update_points_position()
    {
        height = (online ? sync.Height.Value : height);
        width = (online ? sync.Width.Value : width);
        vertices2d[0] = new Vector2(0.0f, -0.001f);
        linerender.enabled = false;
        for (int i = 0; i < nbr_points; i++) //Pour chaque points...
        {
            Vector3 newPos = new Vector3();
            newPos.x = (width / nbr_points) * i;
            if (MoreDistantData() != 0.0)
                newPos.y = (((float)data[i] / (float)MoreDistantData()) * height); //on calcule la position...
            else
                newPos.y = 0.0f;
            newPos.z = 0.0f;
			vertices2d[i + 1] = newPos; //et on genere les vertices
			if (i == nbr_points - 1)
			{
				vertices2d[nbr_points + 1] = newPos;
				vertices2d[nbr_points + 1].y = -0.001f; //Meme chose ici pour le dernier point.
			}
            linerender.SetPosition(i, newPos);
        }
        linerender.enabled = true;
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

	void Update_Collider() //On scale le box collider, important pour le raycast en dessous..
	{
		transform.parent.GetComponent<BoxCollider>().center = new Vector3(width / 2.0f, height / 2.0f);
		transform.parent.GetComponent<BoxCollider>().size = new Vector3(width, height);
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
	void Update()
    {
        if (online)
            GetComponent<MeshRenderer>().material.color = new Color(sync.Color_R.Value,
                sync.Color_G.Value, sync.Color_B.Value);
		if (nbr_points_save != nbr_points)
			Restart();
		if (raycast)
			data_selected = true;
		else
			data_selected = false;
		Update_Selected_Data(hit.point);
        raycast = false;
	}
	
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
