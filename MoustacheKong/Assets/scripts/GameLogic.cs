using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	public Camera Camera2D;
	public Camera Camera3D;

	// Toggle gameMode 
	private bool gameMode3D = false;

	// Use this for initialization
	void Start () {
		// Defaults 2D camera
		Camera2D.enabled = true;
		Camera3D.enabled = false;
	//	GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterController>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/** 
	 * Physics (and controls) stuff goes here.
	 **/
	void FixedUpdate() {
		if (Input.GetKeyDown(KeyCode.C)) {
			gameMode3D = !gameMode3D;
			if(gameMode3D) {
				changeTo3D();
			} else {
				changeTo2D();
			}

		}
	}

	/**
	 * Changes the game to 2D mode.
	 * - Toggles the camera to ortographic
	 * - Activates coliders on stairs
	 **/
	void changeTo2D() {
		Camera2D.enabled = true;
		Camera3D.enabled = false;
//		thirdPersonController.enabled = false;
		//GameObject.FindGameObjectWithTag ("Player").GetComponent(CharacterController).enabled = false;
		gameObject.GetComponent<CharacterController> ().enabled = false;
	}


	/**
	 * Changes the game to 3D mode.
	 * - Toggle the camera to perspective
	 * - Deactivates coliders on stairs
	 */
	void changeTo3D() {
		Camera3D.enabled = true;
		Camera2D.enabled = false;
		Debug.Log("I'm attached to " + gameObject.name);
		gameObject.GetComponent<CharacterController> ().enabled = true;
	}
}
