﻿using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
	
	public Camera Camera2D;
	public Camera Camera3D;
	
	// Toggle gameMode 
	private bool gameMode3D = false;
	
	// Flag that controls change animation 
	private bool cameraAnimationTo3D = false;
	private float timeAtStartOf3DAnimation = 0.0f;
	private float MAX_ANIMATION_TIME = 0.4f;
	private Matrix4x4 ortho, perspective;
	public float fov = 60f, near = .3f, far  = 1000f, orthographicSize = 25.8f;
	private float aspect;
	//	private MatrixBlender blender;
	
	// Use this for initialization
	void Start () {
		// Defaults 2D camera
		Camera2D.enabled = true;
		Camera3D.enabled = false;
		//	GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterController>().enabled = false;
		
		aspect = (float) Screen.width / (float) Screen.height;

		ortho = Camera2D.projectionMatrix;
			//Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
		perspective = Matrix4x4.Perspective(fov, aspect, near, far);
	//	Camera2D.projectionMatrix = ortho;
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraAnimationTo3D && MaxAnimationTimeReached()) {
			Debug.Log("END REACHED");

			changeTo3D();
			cameraAnimationTo3D = false;

			StopAllCoroutines();
			StartCoroutine(LerpFromTo(Camera2D.projectionMatrix, ortho, 1f));
		}
	}
	
	/** 
	 * Physics (and controls) stuff goes here.
	 **/
	void FixedUpdate() {
		if (Input.GetKeyDown(KeyCode.C)) {
			gameMode3D = !gameMode3D;
			if(gameMode3D) {
				Debug.Log("Animation Started!");
				if(!cameraAnimationTo3D){
					//changeTo3D();
					cameraAnimationTo3D = true;
					timeAtStartOf3DAnimation = Time.time;
					
					StopAllCoroutines();
					StartCoroutine(LerpFromTo(Camera2D.projectionMatrix, perspective, 1f));
				}
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
	
	bool MaxAnimationTimeReached(){
		return (Time.time - timeAtStartOf3DAnimation) >= MAX_ANIMATION_TIME; 
	}
	
	public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time){
		Matrix4x4 ret = new Matrix4x4();
		for (int i = 0; i < 16; i++) {
			ret [i] = Mathf.Lerp (from [i], to [i], time);
		}
		return ret;
	}
	
	private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration){
		float startTime = Time.time;
		while (Time.time - startTime < duration){
			Camera2D.projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration);
			yield return 1;
		}
		
		Camera2D.projectionMatrix = dest;
	}
}
