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
	private Mesh mesh;
	private Vector2[] vertices2d;
    // Use this for initialization

	void Restart()
	{
		StopAllCoroutines();
		Start();
	}
    void Start()
    {

		nbr_points_save = nbr_points;
        initial_pos = transform.position;
        linerender = GetComponent<LineRenderer>();
        linerender.SetVertexCount(nbr_points);
        data = new double[nbr_points];
        for (int i = 0; i < nbr_points; i++)
            data[i] = 0.0f;
		mesh = new Mesh();
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

    IEnumerator FakeValues()
    {
        while (true)
        {
			double d = data[nbr_points - 1] + (double)Random.Range(-2.0f, 2.0f);
			d = (d > 10.0 ? 10.0 : d);
            InsertData((d < 0.0 ? 0.0 : d));
            Update_points_position();
            yield return new WaitForSeconds(time_to_update);
        }
    }

    void Update_points_position()
    {
		if (nbr_points_save != nbr_points)
			Restart();
        transform.position = initial_pos;
		vertices2d[0] = transform.position;
        for (int i = 0; i < nbr_points; i++)
        {
            Vector3 newPos = new Vector3();
            newPos.x = (width / nbr_points) * i;
            if (MoreDistantData() != 0.0)
                newPos.y = (((float)data[i] / (float)MoreDistantData()) * height);
            newPos.z = 0.0f;
			vertices2d[i + 1] = newPos;
			if (i == 0)
			{
				vertices2d[0] = newPos;
				vertices2d[0].y = 0.0f;
				vertices2d[0] = new Vector2(0.0f, 0.0f);
			}
			if (i == nbr_points - 1)
			{
				vertices2d[nbr_points + 1] = newPos;
				vertices2d[nbr_points + 1].y = 0.0f;
			}
            linerender.SetPosition(i, newPos);
        }
		Triangulator tr = new Triangulator(vertices2d);
		int[] indices = tr.Triangulate();
		Vector3[] vertices = new Vector3[vertices2d.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = new Vector3(vertices2d[i].x, vertices2d[i].y, 0);
		}
		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		GetComponent<MeshFilter>().mesh = mesh;
		//transform.position = new Vector3(transform.position.x,
		//     transform.position.y + (first_point_position - last_point_position) / 2.0f, transform.position.z);

	}

    // Update is called once per frame
    void Update()
    {

    }
}
