using UnityEngine;
using System.Collections;

public class wallController : MonoBehaviour {
	
	public GameObject topWall, bottomWall, rightWall, leftWall;

	// Use this for initialization
	void Start () {

		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		WorldController worldScript = gameController.GetComponent<WorldController> ();

		float wallHeight = worldScript.height + 4.0f ;
		float y_pos = -worldScript.height + (wallHeight / 2.0f);

		topWall.transform.position = new Vector3 (0.0f, y_pos, worldScript.width/2.0f);
		bottomWall.transform.position = new Vector3 (0.0f, y_pos, -worldScript.width/2.0f);
		rightWall.transform.position = new Vector3 (worldScript.width/2.0f, y_pos, 0.0f);
		leftWall.transform.position = new Vector3 (-worldScript.width/2.0f, y_pos, 0.0f);

		topWall.transform.localScale = new Vector3(worldScript.width, wallHeight, 0.5f);
		bottomWall.transform.localScale = new Vector3(worldScript.width, wallHeight, 0.5f);
		rightWall.transform.localScale = new Vector3(0.5f, wallHeight, worldScript.width);
		leftWall.transform.localScale = new Vector3(0.5f, wallHeight, worldScript.width);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
