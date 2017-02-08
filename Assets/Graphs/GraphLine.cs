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
    private Vector3 initial_pos;
    private LineRenderer linerender;
	private Vector2[] vertices2d;

	void Restart()
	{
		StopAllCoroutines();
		Start();
	}

    void Start() //Initialisation..
    {

		nbr_points_save = nbr_points;
        initial_pos = transform.position;
        linerender = GetComponent<LineRenderer>();
        linerender.SetVertexCount(nbr_points);
        data = new double[nbr_points];
        for (int i = 0; i < nbr_points; i++) //On remplit les donnees de zero..
            data[i] = 0.0f;
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
        for (int i = 0; i < nbr_points - 1; i++)
            data[i] = data[i + 1];
        data[nbr_points - 1] = d;
    }

    IEnumerator FakeValues() //Coroutine pour ajouter regulierement des fausses valeurs au graph
    {
        while (true)
        {
			double d = data[nbr_points - 1] + (double)Random.Range(-0.5f, 0.5f);
			d = (d > 10.0 ? 10.0 : d);
            InsertData((d < 0.0 ? 0.0 : d));
            Update_points_position();
            yield return new WaitForSeconds(time_to_update);
        }
    }

	/* 
	 * Fonction permettant d'update dans le world la position des points
	 * ainsi que de generer un mesh a la volee afin de "dessiner" le support en dessous les bars
	 */
    void Update_points_position()
    {
		vertices2d[0] = new Vector2(0.0f, -0.005f);
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
				vertices2d[nbr_points + 1].y = -0.005f; //Meme chose ici pour le dernier point.
			}
            linerender.SetPosition(i, newPos);
        }
		Mesh_Generator m = new Mesh_Generator(vertices2d, this.gameObject); //On genere le mesh !
		Update_cylinder();
	}

	void Update_cylinder() //Deformation et positionnement des supports cylindrique
	{
		transform.FindChild("CylinderX").transform.localScale = new Vector3(0.01f, width / 2.0f, 0.01f);
		transform.FindChild("CylinderX").transform.localPosition = new Vector3(width / 2.0f, 0.0f, 0.0f);
		transform.FindChild("CylinderY").transform.localScale = new Vector3(0.01f, height / 2.0f, 0.01f);
		transform.FindChild("CylinderY").transform.localPosition = new Vector3(0.0f, height / 2.0f, 0.0f);
	}

	// Update is called once per frame
    void Update()
    {
		if (nbr_points_save != nbr_points)
			Restart();
	}
}
