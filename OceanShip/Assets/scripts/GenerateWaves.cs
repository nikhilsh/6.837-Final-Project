using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateWaves : MonoBehaviour, AudioScript.AudioCallbacks{

	Mesh waterMesh;

	//The new values of the vertices after we applied the wave algorithm are stored here
	private Vector3[] newVertices;
	//The initial values of the vertices are stored here
	private Vector3[] originalVertices;

	private WaveController waveScript;

	void Start() {
		//Get the water mesh

		waterMesh = this.GetComponent<MeshFilter>().mesh;

		GenerateWaterMesh.GenerateWater (this.GetComponent<MeshFilter>(), 10.0f, 0.5f, 0.0f);
		waterMesh = this.GetComponent<MeshFilter>().mesh;

		originalVertices = waterMesh.vertices;

		//Get the waveScript
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");

		waveScript = gameController.GetComponent<WaveController>();

//		AudioScript processor = FindObjectOfType<AudioScript>();
//		processor.addAudioCallback(this);


//		waterMesh = CreateMesh ();
	}


	void Update() {
		MoveSea();
	}

	public void onOnbeatDetected()
	{
		Debug.Log("Beat!!!");
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
