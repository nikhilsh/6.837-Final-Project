using UnityEngine;
using System.Collections;

public class seabottom : MonoBehaviour {

	Mesh seabedMesh;
	private WorldController worldScript;

	// Use this for initialization
	void Start () {
		//Get the controllerScripts
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		worldScript = gameController.GetComponent<WorldController> ();

		//Get and update seabed mesh
		seabedMesh = this.GetComponent<MeshFilter>().mesh;
		GenerateWaterMesh.GenerateWater (this.GetComponent<MeshFilter>(), worldScript.width, 0.5f, 0.0f);
		seabedMesh = this.GetComponent<MeshFilter>().mesh;

		this.transform.position = new Vector3 (0.0f, -worldScript.height, 0.0f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
