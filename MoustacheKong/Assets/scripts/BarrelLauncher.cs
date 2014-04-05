using UnityEngine;
using System.Collections;

public class BarrelLauncher : MonoBehaviour {

	// 
	public GameObject barrel;
	float lastTime = 0f;
	float time = 0f;
	float nextBarrel = 0f;
	public float minRandom = 0.8f;
	public float maxRandom = 4f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// Increment timer
		time += Time.deltaTime;
		if(time - lastTime > nextBarrel) {
			// Reset the timer.
			lastTime = time;
			// Launch a barrel with barrel mode.
			launchBarrel(true);
			// Calculate the next barrel.
			nextBarrel = Random.Range(minRandom, maxRandom);
		}
	}

	/// <summary>
	/// Launchs the barrel.
	/// </summary>
	/// <param name="type">If set to <c>true</c> it's a normal barrel.</param>
	void launchBarrel(bool type) {
		//TODO: Include kong animation here.
		GameObject go = (GameObject) Instantiate (barrel);
		int lane = Random.Range (0f, 1f) > 0.5f ? 1 : 2;
		go.SendMessage ("setLane", lane);
	}

}
