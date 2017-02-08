using UnityEngine;
using System.Collections;

/* Classe pour generer des mesh a partir des vertices, elle s'occupe de trianguler, generer les normals map et creer le mesh :) */
public class Mesh_Generator {

	public Mesh_Generator(Vector2[] vertices2d, GameObject gameobject)
	{
		Apply_Mesh(Generate_Mesh_from_2dvertices(vertices2d), gameobject);
	}

	public void Apply_Mesh(Mesh mesh, GameObject gameobject)
	{
		gameobject.GetComponent<MeshFilter>().mesh.Clear();
		gameobject.GetComponent<MeshFilter>().mesh = mesh;
	}

	public Mesh Generate_Mesh_from_2dvertices(Vector2[] vertices2d)
	{
		Mesh mesh = new Mesh();
		Triangulator tr = new Triangulator(vertices2d);
		int[] indices = tr.Triangulate();
		Vector3[] vertices = new Vector3[vertices2d.Length];
		for (int i = 0; i < vertices.Length; i++) //Reconversion en  3d
		{
			vertices[i] = new Vector3(vertices2d[i].x, vertices2d[i].y, 0);
		}
		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return (mesh);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
