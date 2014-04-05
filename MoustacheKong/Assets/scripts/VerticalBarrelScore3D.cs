using UnityEngine;
using System.Collections;

public class VerticalBarrelScore3D : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		// Get the game state (2d or 3d)
		bool camera3D = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled;
		if(other.tag == "Player") {
			GameObject p = GameObject.FindGameObjectWithTag ("Player");
			if(camera3D) {
				Debug.Log("Jumped 3D");
				p.SendMessage ("jumpedBarrel", 2);
			}
		}
	}
}
