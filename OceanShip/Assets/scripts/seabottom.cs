using UnityEngine;
using System.Collections;

public class seabottom : MonoBehaviour {
	// Make seabottom same size
	//The total size in m
	float waterWidth = 10.0f;
	//Width of one square (= distance between vertices)
	float gridSpacing = 1.0f;

	// Use this for initialization
	void Start () {
		// Seabottom
		MeshFilter seabottomMeshFilter = this.GetComponent<MeshFilter>();

		GenerateWaterMesh.GenerateWater(seabottomMeshFilter, waterWidth, gridSpacing, -5.0f);

		transform.position = new Vector3(-waterWidth/2.0f, -3.0f, -waterWidth/2.0f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
