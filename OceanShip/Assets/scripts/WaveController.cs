using UnityEngine;
using System.Collections;

public class WaveController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float scale = 0.3f;
	public float speed = 1.2f;
	public float waveDistance = 1f;
	public float noiseStrength = 1.3f;
	public float noiseWalk = 1.3f;

	public float GetWaveYPos(float x_coord, float z_coord) {
		float y_coord = 0f;
		y_coord += Mathf.Sin((Time.time * speed + x_coord + y_coord + z_coord ) / waveDistance) * scale;
		y_coord += Mathf.PerlinNoise(x_coord + noiseWalk, z_coord + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;

		return y_coord;
	}		
}
