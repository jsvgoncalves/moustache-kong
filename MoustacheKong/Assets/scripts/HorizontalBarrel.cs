using UnityEngine;
using System.Collections;

/// <summary>
/// Horizontal barrel
/// Kills the player on hit if game mode is 2D.
/// </summary>
public class HorizontalBarrel : MonoBehaviour {
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other) {
		// Get the game state (2d or 3d)
		bool camera3D = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled;
		if(other.tag == "Player") {
			GameObject p = GameObject.FindGameObjectWithTag ("Player");
			// If the game mode is 2D then kill the player.
			if(camera3D) {
				p.SendMessage ("jumpedBarrel", 1);
			} else {
				p.SendMessage ("barrelHit", 1);
			}
		}
	}
}
