using UnityEngine;
using System.Collections;

/// <summary>
/// Barrel hit.
/// This handles the trigger events on the barrel itself.
/// Kills the player on hit.
/// </summary>
public class BarrelHit : MonoBehaviour {
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other) {
		if( other.tag == "Player") {
			GameObject p = GameObject.FindGameObjectWithTag ("Player");
			bool camera3D = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled;
			if(camera3D) {
				p.SendMessage ("barrelHit", 1);
			}
		}
	}
}
