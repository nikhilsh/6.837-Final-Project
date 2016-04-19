using UnityEngine;
using System.Collections;

public class WaveController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Wave parameters

	//Wave height and speed
	public float scale = 0.4f;
	public float speed = 1.5f;
	//The width between the wave peaks
	public float waveDistance = 1.4f;
	//Noise parameters
	public float noiseStrength = 1f;
	public float noiseWalk = 1.3f;

	//Basic idea from http://answers.unity3d.com/questions/443031/sinus-for-rolling-waves.html
	public float GetWaveYPos(float x_coord, float z_coord) {
		float y_coord = 0f;

		//Using only x_coord or z_coord will produce straight waves
		//Using only vertex.y will produce an up/down movement
		//x_coord + y_coord + z_coord rolling waves sideways
		//x_coord * z_coord produces a moving sea without rolling waves
		y_coord += Mathf.Sin((Time.time * speed + z_coord) / waveDistance) * scale;

		//Add noise to make it more realistic
//		y_coord += Mathf.PerlinNoise(x_coord + noiseWalk, z_coord + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;

		return y_coord;
	}		
}
