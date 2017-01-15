using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlaneGenerator : EditorWindow {

	private int width;
	private int height;
	private bool applyPerlinNoise;
	private float perlinScale;
	private float perlinSeed;
	private Material mat;

	[MenuItem("Custom/Plane Generator")]
	public static void OpenPlaneGenerator()
	{
		EditorWindow.GetWindow (typeof(PlaneGenerator));
	}

	void OnGUI()
	{
		GUILayout.BeginVertical ();
	
		width = EditorGUILayout.IntField ("Width", width);
		height = EditorGUILayout.IntField ("Height", height);
		mat = (Material)EditorGUILayout.ObjectField ("Material", mat, typeof(Material));
		applyPerlinNoise = EditorGUILayout.Toggle ("Apply Perlin Noise", applyPerlinNoise);
		perlinScale = EditorGUILayout.FloatField ("Perlin Height Scale", perlinScale);
		perlinSeed = EditorGUILayout.FloatField ("Perlin Seed", perlinSeed);
			
		if (GUILayout.Button ("Generate"))
			GeneratePlane ();

		GUILayout.EndVertical ();
	}

	private void GeneratePlane()
	{
		GameObject plane = new GameObject ("Plane");
		Mesh planeMesh = new Mesh ();
		planeMesh.name = "Plane";

		List<Vector3> verts = new List<Vector3>(width*height);
		List<Vector3> normals = new List<Vector3>(width*height);
		List<Vector2> uvs = new List<Vector2>(width*height);
		List<int> triangles = new List<int> ();

		float vertHeight = 0.0f;

		for (float i = 0; i < height; i++) {
			for (float j = 0; j < width; j++) {

				if (applyPerlinNoise)
					vertHeight = (2 * Mathf.PerlinNoise (j/(float)width + perlinSeed, i/(float)height + perlinSeed) - 1) * perlinScale;
				else
					vertHeight = 0.0f;
				
				verts.Add (new Vector3 (i, vertHeight, j));
				uvs.Add (new Vector3 (i / (float)height, j / (float)width));
				normals.Add (Vector3.up);
			}
		}

		for (int i = 0; i < height*width - width - 1; i++) {
			if ((i+1) % width != 0) {
				triangles.Add (i);
				triangles.Add (i + 1);
				triangles.Add (i + width);

				triangles.Add (i + 1);
				triangles.Add (i + 1 + width);
				triangles.Add (i + width);
			}
		}

		planeMesh.SetVertices (verts);
		planeMesh.SetNormals (normals);
		planeMesh.SetUVs (0, uvs);
		planeMesh.SetUVs (1, uvs);
		planeMesh.SetUVs (2, uvs);
		planeMesh.SetUVs (3, uvs);

		planeMesh.SetTriangles (triangles, 0);

		plane.AddComponent<MeshFilter> ();
		plane.GetComponent<MeshFilter> ().mesh = planeMesh;

		plane.AddComponent<MeshRenderer> ();

		if(mat != null)
			plane.GetComponent<MeshRenderer> ().material = mat;
	}
}
