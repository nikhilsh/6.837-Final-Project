using UnityEngine;
using System.Collections;

public class dropItems : MonoBehaviour {
	public GameObject newGameObject;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CreateWaterWakesWithMouse ();
	}

	//Interact with the water wakes by clicking with the mouse
	void CreateWaterWakesWithMouse() {
		//Fire ray from the current mouse position
		if (Input.GetKey(KeyCode.Mouse0)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {

				//Convert the mouse position from global to local
				Vector3 localPos = transform.InverseTransformPoint(hit.point);
				GameObject temp = Instantiate(newGameObject);
				temp.transform.position = localPos;

			}
		}
	}
}
