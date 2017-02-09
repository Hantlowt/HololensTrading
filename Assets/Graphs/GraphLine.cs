using UnityEngine;
using System.Collections;

public class GraphLine : MonoBehaviour
{

    public int nbr_points = 50;
	private int nbr_points_save;
	public float height = 1.0f;
    public float width = 1.0f;
	public float time_to_update = 0.5f;
    public double[] data;
	public string graph_name;
    private LineRenderer linerender;
	private Vector2[] vertices2d;
	private bool data_selected = false;
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
        for (int i = 1; i < nbr_points; i++) //On remplit les donnees de zero..
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
			linerender.endColor = Color.green;
		else
			linerender.endColor = Color.red;
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
			Update_All();
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

	int Nearest_Points (Vector3 pos)
	{
		float dist = Mathf.Infinity;
		int result_id = 0;
		pos.y = 0.0f;
		for (int i = 0; i < nbr_points; i++)
		{
			Vector3 pos_point = transform.position + linerender.GetPosition(i);
			pos_point.y = 0.0f;
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
		if (data_selected)
		{
			Selected_Data.gameObject.SetActive(true);
			Update_Selected_Data(hit.point);
		}
		else
			Selected_Data.gameObject.SetActive(false);
	}

	void Update_Selected_Data(Vector3 pos)
	{
		int id = Nearest_Points(pos);
		float def = (width < height ? width / 10.0f : height / 10.0f);
		Selected_Data.GetComponent<TextMesh>().text = data[id].ToString();
		Selected_Data.position = transform.position + linerender.GetPosition(id);
		Selected_Data.localScale = new Vector3(def, def);
		Vector3 pos_point_line = transform.position + linerender.GetPosition(id);
		pos.z -= 0.2f;
		Selected_Data.GetComponent<LineRenderer>().SetPosition(0, pos_point_line);
		pos_point_line.y -= linerender.GetPosition(id).y;
		Selected_Data.GetComponent<LineRenderer>().SetPosition(1, pos_point_line);
	}
}
