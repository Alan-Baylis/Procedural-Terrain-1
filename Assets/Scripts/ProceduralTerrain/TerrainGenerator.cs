using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour {

	public bool m_ApplyPerlinNoise = false;
	private bool m_Reset = true;
	private bool m_Init = false;

	[System.Serializable]
	public struct PerlinTerrainLevel
	{
		public float frequency;
		public float seed;
	};

	public float m_Scale;	
	public List<PerlinTerrainLevel> m_DetailLevels;

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

		m_Init = true;
	}
		
	void OnValidate()
	{
		if (m_ApplyPerlinNoise) {
			UpdateTerrain ();
			m_Reset = false;
		} else if (!m_Reset) {
			ResetTerrain ();
			m_Reset = true;
		}
	}

	public void UpdateTerrain()
	{
		if (!m_Init) {
			Init ();
		} 

		if (m_Init) {
			float vertHeight = 0.0f;
			m_Verts.Clear ();

			for (int i = 0; i < m_DetailLevels.Count; i++) {
				for (int j = 0; j < height; j++) {
					for (int k = 0; k < width; k++) {

						vertHeight = Mathf.PerlinNoise (
							((float)k / (float)width) * m_DetailLevels [i].frequency + m_DetailLevels [i].seed,
							((float)j / (float)height) * m_DetailLevels [i].frequency + m_DetailLevels [i].seed);

						vertHeight = 2 * vertHeight - 1;
						vertHeight *= m_Scale;
						vertHeight /= Mathf.Pow (2, i);
				
						if(i == 0)
							m_Verts.Add (new Vector3 ((float)j, vertHeight, (float)k));
						else
							m_Verts [j * width + k] += new Vector3 (0, vertHeight, 0);
					}
				}
			}

			m_Terrain.SetVertices (m_Verts);
			m_Terrain.RecalculateNormals ();
		}
	}

	public void ResetTerrain()
	{
		if (!m_Init) {
			Init ();
		} 

		if (m_Init) {
			m_Verts.Clear ();

			for (float i = 0; i < height; i++) {
				for (float j = 0; j < width; j++) {
					m_Verts.Add (new Vector3 (i, 0, j));
				}
			}

			m_Terrain.SetVertices (m_Verts);
			m_Terrain.RecalculateNormals ();
		}
	}
}
