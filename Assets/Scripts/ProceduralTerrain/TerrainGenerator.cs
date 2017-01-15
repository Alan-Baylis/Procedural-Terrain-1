using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour {

	public bool m_ApplyPerlinNoise = false;

	public float m_Frequency;
	public float m_Seed;
	public float m_Scale;

	[HideInInspector]
	public int width, height;

	private Mesh m_Terrain;
	private List<Vector3> m_Verts;

	void Start()
	{
		Init ();
	}

	void OnEnable()
	{
		Init ();
	}

	private void Init()
	{
		m_Terrain = GetComponent<MeshFilter> ().sharedMesh;
		m_Verts = new List<Vector3> (width * height);
	}
		
	void OnValidate()
	{
		UpdateTerrain ();
	}

	public void UpdateTerrain()
	{
		if (m_Terrain == null || m_Verts == null) {
			Init ();
		} else {
			float vertHeight = 0.0f;
			m_Verts.Clear ();

			for (float i = 0; i < height; i++) {
				for (float j = 0; j < width; j++) {

					if (m_ApplyPerlinNoise) {
						vertHeight = Mathf.PerlinNoise (
							(j / (float)width) * m_Frequency + m_Seed,
							(i / (float)height) * m_Frequency + m_Seed);

						vertHeight = 2 * vertHeight - 1;
						vertHeight *= m_Scale;
					}
					else
						vertHeight = 0.0f;
				
					m_Verts.Add (new Vector3 (i, vertHeight, j));
				}
			}

			m_Terrain.SetVertices (m_Verts);
			m_Terrain.RecalculateNormals ();
		}
	}
}
