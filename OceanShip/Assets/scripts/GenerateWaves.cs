using UnityEngine;
using System.Collections;

public class GenerateWaves : MonoBehaviour, AudioScript.AudioCallbacks{

	//The water mesh
	Mesh waterMesh;

	//The new values of the vertices after we applied the wave algorithm are stored here
	private Vector3[] newVertices;
	//The initial values of the vertices are stored here
	private Vector3[] originalVertices;

	//To get the y position
	private WaveController waveScript;

	void Start() {
		//Get the water mesh
		waterMesh = this.GetComponent<MeshFilter>().mesh;

		originalVertices = waterMesh.vertices;

		//Get the waveScript
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");

		waveScript = gameController.GetComponent<WaveController>();

		AudioScript processor = FindObjectOfType<AudioScript>();
		processor.addAudioCallback(this);
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

			//Now we need to modify this coordinate's y-position
			//From local to global
			vertice = transform.TransformPoint(vertice);

			vertice.y += waveScript.GetWaveYPos(vertice.x, vertice.z);

			//From global to local
			newVertices[i] = transform.InverseTransformPoint(vertice);
		}

		//Add the new position of the water to the water mesh
		waterMesh.vertices = newVertices;
		//After modifying the vertices it is often useful to update the normals to reflect the change
		waterMesh.RecalculateNormals();
	}
}
