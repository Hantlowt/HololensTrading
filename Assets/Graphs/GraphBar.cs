using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphBar : MonoBehaviour {

    public int nbr_bar = 50; //Nombre de bar dans le graph
    private int nbr_bar_save;
    public float height = 1.0f; //hauteur de celui-ci
    public float width = 1.0f; //largeur de celui-ci
    public float time_to_update = 0.5f; //Pour les besoins de la demo, temps qui s'ecoulent avant l'add de data
    public double[] data; //Les fameuses data
    private GameObject[] bars;
    public GameObject bar_prefab;
    public string graph_name; //Nom du graphique (peut etre modifie a n'importe quel moment dans Unity)
    private Transform CylinderX;
    private Transform CylinderY;
    private Transform Name;
    public Color normal_color;
    public Color select_color;

    void Restart()
    {
        StopAllCoroutines();
        Start();
    }

    void Start() //Initialisation..
    {
        CylinderX = transform.FindChild("CylinderX");
        CylinderY = transform.FindChild("CylinderY");
        Name = transform.FindChild("Name");
        nbr_bar_save = nbr_bar;
        data = new double[nbr_bar];
        if (bars != null)
            for (int i = 0; i < bars.Length; i++)
                Destroy(bars[i]);
        bars = new GameObject[nbr_bar];
        data[0] = 5.0f;
        for (int i = 1; i < nbr_bar; i++) //On remplit les donnees avec des nombres fictifs mais cohérents.
            data[i] = data[i - 1] + Random.Range(-0.5f, 0.5f);
        for (int i = 0; i < nbr_bar; i++)
        {
            bars[i] = Instantiate(bar_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            bars[i].transform.parent = this.transform;
            bars[i].transform.localScale = new Vector3(width / nbr_bar, 1.0f, width / nbr_bar);
            bars[i].transform.localPosition = new Vector3(i * bars[i].transform.localScale.x, 0.0f, 0.0f);
            bars[i].GetComponent<Bar>().data = data[i];
            bars[i].GetComponent<Bar>().normal_color = normal_color;
            bars[i].GetComponent<Bar>().select_color = select_color;
        }
		Update_Name();
		Update_Collider();
		StartCoroutine("FakeValues");
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

    void Update_All()
    {
        Update_bar();
        Update_cylinder();
    }

    IEnumerator FakeValues() //Coroutine pour ajouter regulierement des fausses valeurs au graph
    {
        while (true)
        {
            double d = data[nbr_bar - 1] + (double)Random.Range(-0.5f, 0.5f);
            d = d > 10.0 ? 10.0 : d;
            InsertData(d < 0.0 ? 0.0 : d);
            Update_All(); //Et remettre a jour le graph
            yield return new WaitForSeconds(time_to_update);
        }
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
            bars[i].GetComponent<Bar>().data = data[i];
        }
    }

    void Update_cylinder() //Deformation et positionnement des supports cylindriques
    {
        CylinderX.transform.localScale = new Vector3(0.01f, width / 2.0f, 0.01f);
        CylinderX.transform.localPosition = new Vector3(width / 2.0f, 0.0f, 0.0f);
        CylinderY.transform.localScale = new Vector3(0.01f, height / 2.0f, 0.01f);
        CylinderY.transform.localPosition = new Vector3(0.0f, height / 2.0f, 0.0f);
    }

    void Update_Name() //On change le nom du graph et on le scale correctement
    {
        float def = width < height ? width / 10.0f : height / 10.0f;
        Name.transform.localScale = new Vector3(def, def);
        Name.GetComponent<TextMesh>().text = graph_name;
    }

	void Update_Collider () //On scale le box collider à la taille du graphique en cours
    {
        GetComponent<BoxCollider>().center = new Vector3(width / 2.0f, height / 2.0f);
        GetComponent<BoxCollider>().size = new Vector3(width, height);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (nbr_bar_save != nbr_bar)
            Restart();
    }
}
