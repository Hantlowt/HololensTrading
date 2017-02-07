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
    // Use this for initialization
    void Start()
    {
		nbr_points_save = nbr_points;
        initial_pos = transform.position;
        linerender = GetComponent<LineRenderer>();
        linerender.SetVertexCount(nbr_points);
        data = new double[nbr_points];
        for (int i = 0; i < nbr_points; i++)
            data[i] = 0.0f;
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
		{
			StopAllCoroutines();
			Start();
		}
        float first_point_position = 0.0f;
        float last_point_position = 0.0f;
        transform.position = initial_pos;
        for (int i = 0; i < nbr_points; i++)
        {
            Vector3 newPos = new Vector3();
            newPos.x = (width / nbr_points) * i;
            if (MoreDistantData() != 0.0)
                newPos.y = (((float)data[i] / (float)MoreDistantData()) * height);
            newPos.z = 0.0f;
            if (i == 0)
                first_point_position = newPos.y;
            if (i == nbr_points - 1)
                last_point_position = newPos.y;
            linerender.SetPosition(i, newPos);
        }
        //transform.position = new Vector3(transform.position.x,
        //     transform.position.y + (first_point_position - last_point_position) / 2.0f, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
