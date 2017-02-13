﻿using UnityEngine;
using System.Collections;

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
	public float time_to_update = 0.5f; //Pour les besoins de la demo, temps qui s'ecoulent avant l'add de data
    public double[] data; //Les fameuses data
	public string graph_name; //Nom du graphique, peut-etre modifie a n'importe quel moment
    private LineRenderer linerender;
	private Vector2[] vertices2d;
	private bool data_selected = false; //Lorsque l'on regarde le graph, les donnees s'affichent.. ou non
	private Transform CylinderX;
	private Transform CylinderY;
	private Transform Name;
	private Transform Selected_Data;

	void Restart()
	{
		StopAllCoroutines();
		Start();
	}

    void Start() //Initialisation..
    {
		Selected_Data = transform.FindChild("Selected_Data");
		CylinderX = transform.FindChild("CylinderX");
		CylinderY = transform.FindChild("CylinderY");
		Name = transform.FindChild("Name");
		nbr_points_save = nbr_points;
        linerender = GetComponent<LineRenderer>();
		linerender.numPositions = nbr_points;
        data = new double[nbr_points];
		data[0] = 5.0f;
        for (int i = 1; i < nbr_points; i++) //On remplit les donnes avec n'importe quoi
            data[i] = data[i - 1] + Random.Range(-0.5f, 0.5f);
		vertices2d = new Vector2[nbr_points + 2];
        StartCoroutine("FakeValues");
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
		Update_Name();
		Update_Collider();
	}

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

	/* 
	 * Fonction permettant d'update dans le world la position des points
	 * ainsi que de generer un mesh a la volee afin de "dessiner" le support en dessous les bars
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

	void Update_cylinder() //Deformation et positionnement des supports cylindrique
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
			Vector3 pos_point = transform.position + linerender.GetPosition(i);
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
		if (nbr_points_save != nbr_points)
			Restart();
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10)
			&& hit.transform.gameObject == this.gameObject)
			data_selected = true;
		else
			data_selected = false;
		Update_Selected_Data(hit.point);
	}
	
	/* Alors... Cette fonction de barbar permet de faire un truc super classe :
	 * Afficher les donnees du graph !
	 * Bon en gros, si le curseur est passe sur le graph, data_selected est vrai
	 * Ainsi, on affiche le gameobject child de la data selected qui est compose de la valeur que
	 * l'on observe (il faut donc modifie le texte), ainsi que d'un trait indicateur, realise par
	 * un line_renderer. Il faut donc changer le position de ce traitm la position du texte, et
	 * scale le tout a l'echelle du graph.
	 * Aussi, l'utilisation de la fonction Nearest_Point permet de recuperer un id d'un point du graph dont
	 * une position (celle du curseur en l'occurrence) est la plus proche. C'est ce qui permet de savoir
	 * quelle donnees afficher..
	 */ 
	void Update_Selected_Data(Vector3 pos)
	{
		if (data_selected)
		{
			Selected_Data.gameObject.SetActive(true);
			int id = Nearest_Points(pos);
			float def = (width < height ? width / 10.0f : height / 10.0f);
			Selected_Data.GetComponent<TextMesh>().text = data[id].ToString();
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


}
