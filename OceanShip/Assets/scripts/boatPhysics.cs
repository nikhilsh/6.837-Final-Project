using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class boatPhysics : MonoBehaviour {

	public GameObject UnderwaterMeshOBJ;

	private Mesh BoatMesh;
	private Mesh UnderWaterMesh;

	private Vector3[] originalVerticesArray;
	private int[] originalTrianglesArray;

	private List<Vector3> underwaterVertices;
	private List<int> underwaterTriangles;

	private WaveController waveScript;
	private WorldController worldScript;
	private WaterController waterScript;

	Rigidbody boatRB;

	void Start () {
		BoatMesh = GetComponent<MeshFilter>().mesh;
		UnderWaterMesh = UnderwaterMeshOBJ.GetComponent<MeshFilter>().mesh;

		originalVerticesArray = BoatMesh.vertices;
		originalTrianglesArray = BoatMesh.triangles;

		boatRB = GetComponent<Rigidbody>();
		boatRB.maxAngularVelocity = 0.5f;

		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		waveScript = gameController.GetComponent<WaveController>();
		waterScript = GameObject.FindGameObjectWithTag("Water").GetComponent<WaterController>();
	}
	
	// Update is called once per frame
	void Update () {
		GenerateUnderwaterMesh();
	}

	void GenerateUnderwaterMesh() {
		underwaterVertices = new List<Vector3>();
		underwaterTriangles = new List<int>();

		//Loop through all the triangles (3 vertices at a time)
		int i = 0;
		while (i < originalTrianglesArray.Length) {
			
			//Find the distance from each vertice in the current triangle to the water
			Vector3 vertice1, vertice2, vertice3;
			float? distance1, distance2, distance3;

			vertice1 = originalVerticesArray [originalTrianglesArray [i]];
			vertice2 = originalVerticesArray [originalTrianglesArray [i+1]];
			vertice3 = originalVerticesArray [originalTrianglesArray [i+2]];
			distance1 = DistanceToWater (vertice1);
			distance2 = DistanceToWater (vertice2);
			distance3 = DistanceToWater (vertice3);

			i++; i++; i++;

			Distance distanceObject1 = new Distance();
			Distance distanceObject2 = new Distance();
			Distance distanceObject3 = new Distance();

			distanceObject1.distance = (float)distance1; 
			distanceObject1.name = "one";
			distanceObject1.verticePos = vertice1;

			distanceObject2.distance = (float)distance2;
			distanceObject2.name = "two";
			distanceObject2.verticePos = vertice2;

			distanceObject3.distance = (float)distance3;
			distanceObject3.name = "three";
			distanceObject3.verticePos = vertice3;

			//Continue to the next triangle if all triangles are above the water
			if (distance1 > 0f && distance2 > 0f && distance3 > 0f) {
				continue;
			}

			//Continue to the next triangle if there is no water
			if (distance1 == null || distance2 == null || distance3 == null) {
				continue;
			}

			//Sort the 3 vertexes
			List<Distance> allDistancesList = new List<Distance>();
			allDistancesList.Add(distanceObject1);
			allDistancesList.Add(distanceObject2);
			allDistancesList.Add(distanceObject3);

			allDistancesList.Sort();
			allDistancesList.Reverse();

			//All vertices are underwater
			if (allDistancesList[0].distance < 0f && allDistancesList[1].distance < 0f && allDistancesList[2].distance < 0f) {
				//Make sure these coordinates are unsorted
				AddCoordinateToMesh(distanceObject1.verticePos);				
				AddCoordinateToMesh(distanceObject2.verticePos);				
				AddCoordinateToMesh(distanceObject3.verticePos);
			} 

			//One vertice is above the water, the rest is below
			else if (allDistancesList[0].distance > 0f && allDistancesList[1].distance < 0f && allDistancesList[2].distance < 0f) {
				//H is always at position 0
				Vector3 H = allDistancesList[0].verticePos;

				//Left of H is M
				//Right of H is L
				string M_name = "temp";
				if (allDistancesList[0].name == "one") {
					M_name = "three";
				}
				else if (allDistancesList[0].name == "two") {
					M_name = "one";
				}
				else {
					M_name = "two";
				}

				//We also need the heights to water
				float h_H = allDistancesList[0].distance;
				float h_M = 0f;
				float h_L = 0f;

				Vector3 M = Vector3.zero;
				Vector3 L = Vector3.zero;

				//This means M is at position 1 in the List
				if (allDistancesList[1].name == M_name) {
					M = allDistancesList[1].verticePos;
					L = allDistancesList[2].verticePos;

					h_M = allDistancesList[1].distance;
					h_L = allDistancesList[2].distance;
				}
				else {
					M = allDistancesList[2].verticePos;
					L = allDistancesList[1].verticePos;

					h_M = allDistancesList[2].distance;
					h_L = allDistancesList[1].distance;
				}


				// Calculate where we should cut the triangle to form 2 new triangles
				// because the resulting area will always form a square

				//Point I_M
				Vector3 MH = H - M;

				float t_M = -h_M / (h_H - h_M);

				Vector3 MI_M = t_M * MH;

				Vector3 I_M = MI_M + M;


				//Point I_L
				Vector3 LH = H - L;

				float t_L = -h_L / (h_H - h_L);

				Vector3 LI_L = t_L * LH;

				Vector3 I_L = LI_L + L;

				AddCoordinateToMesh(M);
				AddCoordinateToMesh(I_M);
				AddCoordinateToMesh(I_L);

				AddCoordinateToMesh(M);
				AddCoordinateToMesh(I_L);
				AddCoordinateToMesh(L);
			}
			//Two vertices are above the water, the other is below
			else if (allDistancesList[0].distance > 0f && allDistancesList[1].distance > 0f && allDistancesList[2].distance < 0f) {
				Vector3 L = allDistancesList[2].verticePos;

				//Find the name of H
				string H_name = "temp";
				if (allDistancesList[2].name == "one") {
					H_name = "two";
				}
				else if (allDistancesList[2].name == "two") {
					H_name = "three";
				}
				else {
					H_name = "one";
				}

				//We also need the heights to water
				float h_L = allDistancesList[2].distance;
				float h_H = 0f;
				float h_M = 0f;

				Vector3 H = Vector3.zero;
				Vector3 M = Vector3.zero;

				//This means that H is at position 1 in the list
				if (allDistancesList[1].name == H_name) {
					H = allDistancesList[1].verticePos;
					M = allDistancesList[0].verticePos;

					h_H = allDistancesList[1].distance;
					h_M = allDistancesList[0].distance;
				}
				else {
					H = allDistancesList[0].verticePos;
					M = allDistancesList[1].verticePos;

					h_H = allDistancesList[0].distance;
					h_M = allDistancesList[1].distance;
				}
					
				//Point J_M
				Vector3 LM = M - L;

				float t_M = -h_L / (h_M - h_L);

				Vector3 LJ_M = t_M * LM;

				Vector3 J_M = LJ_M + L;


				//Point J_H
				Vector3 LH = H - L;

				float t_H = -h_L / (h_H - h_L);

				Vector3 LJ_H = t_H * LH;

				Vector3 J_H = LJ_H + L;

				//Create the triangle
				AddCoordinateToMesh(L);
				AddCoordinateToMesh(J_H);
				AddCoordinateToMesh(J_M);
			}

			//Calculate the center of the triangle
			Vector3 centerPoint = (vertice1 + vertice2 + vertice3) / 3f;


			//Calculate the distance to the surface from the center of the triangle
			float distance_to_surface = Mathf.Abs((float)DistanceToWater(centerPoint));

			//From localspace to worldspace
			centerPoint = transform.TransformPoint(centerPoint);
			vertice1 = transform.TransformPoint(vertice1);
			vertice2 = transform.TransformPoint(vertice2);
			vertice3 = transform.TransformPoint(vertice3);


			//Calculate the normal to the triangle
			Vector3 crossProduct = Vector3.Cross(vertice2 - vertice1, vertice3 - vertice1).normalized;

			//Calculate the area of the triangle by using Heron's formula
			float a = Vector3.Distance(vertice1, vertice2);
			float c = Vector3.Distance(vertice3, vertice1);

			float area_sin = (a * c * Mathf.Sin(Vector3.Angle(vertice2-vertice1, vertice3-vertice1) * Mathf.Deg2Rad)) / 2f;

			float area = area_sin;		

			//The buoyancy force
			AddBuoyancy(distance_to_surface, area, crossProduct, centerPoint);
			AddWaveDrifting (area, crossProduct, centerPoint);

		}

		//Generate the final underwater mesh
		UnderWaterMesh.Clear();
		UnderWaterMesh.name = "Underwater Mesh";
		UnderWaterMesh.vertices = underwaterVertices.ToArray();
		UnderWaterMesh.triangles = underwaterTriangles.ToArray();

		UnderWaterMesh.RecalculateBounds();
		UnderWaterMesh.RecalculateNormals();
	}

	private void AddBuoyancy(float distance_to_surface, float area, Vector3 crossProduct, Vector3 centerPoint) {

		/*
		 * Hydrostatic Force
		 * dF = rho * g * z * dS * n
		 * rho - density of water
		 * g - gravity
		 * z - distance to surface
		 * dS - surface area
		 * n - normal to the surface
		*/

		Vector3 F = waterScript.density * Physics.gravity.y * distance_to_surface * area * crossProduct;

		F = new Vector3(0f, F.y, 0f);

		boatRB.AddForceAtPosition(F, centerPoint);
	}

	//Find the distance from vertice to water
	//Positive if above water
	//Negative if below water
	float? DistanceToWater(Vector3 position) {	
		//Calculate the coordinate of the vertice in global space
		Vector3 globalVerticePosition = transform.TransformPoint(position);

		float? y_pos = 0.0f;

		y_pos += waveScript.GetWaveYPos(globalVerticePosition.x, globalVerticePosition.z);

		return globalVerticePosition.y - y_pos;
	}

	void AddCoordinateToMesh(Vector3 coord) {
		underwaterVertices.Add(coord);
		underwaterTriangles.Add(underwaterVertices.Count - 1);
	}

	//Will add buoyance so it can float, and drifting from the waves
	void AddForcesToBoat() {		
		int i = 0;
		while (i < underwaterTriangles.Count) {			
			//The position of the vertice in Vector3 format
			Vector3 vertice_1_pos = underwaterVertices[underwaterTriangles[i]];
			i++;

			Vector3 vertice_2_pos = underwaterVertices[underwaterTriangles[i]];
			i++;

			Vector3 vertice_3_pos = underwaterVertices[underwaterTriangles[i]];
			i++;
		}
	}

	private void AddWaveDrifting(float area, Vector3 normal, Vector3 centerPoint) {
		/*
		 * Drift Force
		 * F = 0.5 * rho * g * S * S * n
		 * rho - density of water or whatever medium you have
		 * g - gravity
		 * S - surface area
		 * n - normal to the surface
		*/

		Vector3 F = 0.5f * waterScript.density * Physics.gravity.y * area * area * normal;

		F = new Vector3(F.x, 0f, F.z);

		boatRB.AddForceAtPosition(F, centerPoint);
	}

	void FixedUpdate() {		
		// Will add buoyance so it can float, and drifting from the waves
		if (underwaterTriangles.Count > 0) {
			AddForcesToBoat();
		}
	}
}



//Help class to sort the distances
public class Distance : IComparable<Distance> {
	public float distance;
	public string name;
	public Vector3 verticePos;

	public int CompareTo(Distance other) {
		return this.distance.CompareTo(other.distance);
	}
}

