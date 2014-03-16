using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	public Camera Camera2D;
	public Camera Camera3D;

	// Use this for initialization
	void Start () {
		// Defaults 2D camera
		Camera2D.enabled = true;
		Camera3D.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/** 
	 * Physics (and controls) stuff goes here.
	 **/
	void FixedUpdate() {
		if (Input.GetKeyDown(KeyCode.C)) {
			Camera2D.enabled = !Camera2D.enabled;
			Camera3D.enabled = !Camera3D.enabled;
		}
	}

	/**
	 * Changes the game to 2D mode.
	 * - Toggles the camera to ortographic
	 * - Activates coliders on stairs
	 **/
	void changeTo2D() {

	}


	/**
	 * Changes the game to 3D mode.
	 * - Toggle the camera to perspective
	 * - Deactivates coliders on stairs
	 */
	void changeTo3D() {
	}
}
