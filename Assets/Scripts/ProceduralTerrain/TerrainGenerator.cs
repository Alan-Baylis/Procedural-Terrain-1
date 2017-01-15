using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour {

	public bool m_ApplyPerlinNoise = false;
	public bool m_ApplyDeltaTime = false;

	public float m_PerlinFrequency;
	public float m_PerlinSeed;
	public float m_PerlinScale;

	public int width, height;

	private Mesh m_Terrain;
	private List<Vector3> m_Verts;

	void OnEnable()
	{
		m_Terrain = GetComponent<MeshFilter> ().sharedMesh;
		m_Verts = new List<Vector3> (width * height);
	}

	void Update()
	{
		if(m_ApplyDeltaTime)
			UpdateTerrain(Time.deltaTime);
	}

	void OnValidate()
	{
		UpdateTerrain (1.0f);
	}

	public void UpdateTerrain(float delta)
	{
		float vertHeight = 0.0f;
		m_Verts.Clear ();

		for (float i = 0; i < height; i++) {
			for (float j = 0; j < width; j++) {

				if (m_ApplyPerlinNoise)
					vertHeight = (2 * Mathf.PerlinNoise ((j/(float)width) * m_PerlinFrequency + m_PerlinSeed,
						(i/(float)height) * m_PerlinFrequency + m_PerlinSeed) - 1) * m_PerlinScale;
				else
					vertHeight = 0.0f;
				
				m_Verts.Add (new Vector3 (i, vertHeight, j));
			}
		}

		m_Terrain.SetVertices (m_Verts);
		m_Terrain.RecalculateNormals ();
	}
}
