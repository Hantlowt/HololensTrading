using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphBar : MonoBehaviour {

    public int nbr_points = 50; //Nombre de points dans le graph
    private int nbr_points_save;
    public float height = 1.0f; //hauteur de celui-ci
    public float width = 1.0f; //largeur de celui-ci
    public float time_to_update = 0.5f; //Pour les besoins de la demo, temps qui s'ecoulent avant l'add de data
    public double[] data; //Les fameuses data
    private GameObject[] bars;
    public GameObject bar_prefab;
    public string graph_name; //Nom du graphique, peut-etre modifie a n'importe quel moment
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
        nbr_points_save = nbr_points;
        data = new double[nbr_points];
        if (bars != null)
            for (int i = 0; i < bars.Length; i++)
                Destroy(bars[i]);
        bars = new GameObject[nbr_points];
        data[0] = 5.0f;
        for (int i = 1; i < nbr_points; i++) //On remplit les donnes avec n'importe quoi
            data[i] = data[i - 1] + Random.Range(-0.5f, 0.5f);
        for (int i = 0; i < nbr_points; i++)
        {
            bars[i] = Instantiate(bar_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            bars[i].transform.parent = this.transform;
            bars[i].transform.localScale = new Vector3(width / nbr_points, 1.0f, width / nbr_points);
            bars[i].transform.localPosition = new Vector3(i * bars[i].transform.localScale.x, 0.0f, 0.0f);
            bars[i].GetComponent<Bar>().data = data[i];
            bars[i].GetComponent<Bar>().normal_color = normal_color;
            bars[i].GetComponent<Bar>().select_color = select_color;
        }
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
        
        for (int i = 0; i < nbr_points; i++)
        {
            bars[i].transform.localScale = new Vector3(bars[i].transform.localScale.x,
                (((float)data[i] / (float)MoreDistantData()) * height) / 2.0f, bars[i].transform.localScale.z);
            bars[i].transform.localPosition = new Vector3(bars[i].transform.localPosition.x,
                bars[i].transform.localScale.y, bars[i].transform.localPosition.z);
            bars[i].GetComponent<Bar>().data = data[i];
        }
    }

    void Update_cylinder() //Deformation et positionnement des supports cylindrique
    {
        CylinderX.transform.localScale = new Vector3(0.01f, width / 2.0f, 0.01f);
        CylinderX.transform.localPosition = new Vector3(width / 2.0f, 0.0f, 0.0f);
        CylinderY.transform.localScale = new Vector3(0.01f, height / 2.0f, 0.01f);
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

    // Update is called once per frame
    void Update()
    {
        if (nbr_points_save != nbr_points)
            Restart();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10)
            && hit.transform.gameObject.name == "Bar")
            Debug.Log("sjdkfjlksfd");
    }
}
