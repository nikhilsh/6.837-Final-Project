using UnityEngine;
using System.Collections;

public class WaterController : MonoBehaviour {

	Mesh waterMesh;

	//The new values of the vertices after we applied the wave algorithm are stored here
	private Vector3[] newVertices;
	//The initial values of the vertices are stored here
	private Vector3[] originalVertices;

	private WaveController waveScript;
	private WorldController worldScript;

	public float density = 1000.0f;

	void Start() {
		//Get the controllerScripts
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		waveScript = gameController.GetComponent<WaveController>();
		worldScript = gameController.GetComponent<WorldController> ();

		//Get and update water mesh
		waterMesh = this.GetComponent<MeshFilter>().mesh;
		GenerateWaterMesh.GenerateWater (this.GetComponent<MeshFilter>(), worldScript.width, 0.5f, 0.0f);
		waterMesh = this.GetComponent<MeshFilter>().mesh;

		originalVertices = waterMesh.vertices;
	}
		
	void Update() {
		MoveSea();
	}

	void MoveSea() {
		newVertices = new Vector3[originalVertices.Length];

		for (int i = 0; i < originalVertices.Length; i++) {
			Vector3 vertice = originalVertices[i];

			vertice = transform.TransformPoint(vertice);

			vertice.y += waveScript.GetWaveYPos(vertice.x, vertice.z);

			newVertices[i] = transform.InverseTransformPoint(vertice);
		}
		waterMesh.vertices = newVertices;
		waterMesh.RecalculateNormals();
	}
}
