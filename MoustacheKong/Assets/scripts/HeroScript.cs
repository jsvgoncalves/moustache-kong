﻿	using UnityEngine;
using System.Collections;
	
// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Third Person Player/Third Person Controller")]
	
public class HeroScript : MonoBehaviour
{
	
		// Controlo das Ladders
		private bool touchingLadder = false;
		public float rotationDamping = 30f;
		public float runSpeed = 5f;
		public int gravity = 5;
		public float jumpSpeed = 10;
	
		// Controlo do personagem
		public bool tiltedRightIn3D = true;
		public GameObject Hero2D, Hero2DInverse;
		public GameObject HeroTiltedRight, HeroTiltedRightBackwards, HeroTiltedLeft, HeroTiltedLeftBackwards;
		public bool facingRight = true, facingBackwards = false;
		private int heroDirection = 0;
		// Variaveis que guardam as posicoes (rotacoes) originais do Hero em 3D
		//	private Quaternion originalTiltedRight, originalTiltedLeft, backwardsTiltedLeft, backwardsTiltedRight;
		//	private float timeAtStartOfJump = 0.0f, timeAtStartOfMovement = 0.0f;
		//	private Quaternion originalPosition, alternatePosition;
	
		bool canJump, isJumping = false;
	
		float moveSpeed;
		float verticalVel;  // Used for continuing momentum while in air
		CharacterController controller;
	
		void Start ()
		{
				controller = (CharacterController)GetComponent (typeof(CharacterController));
	
				/*	originalTiltedRight = HeroTiltedRight.transform.rotation;
	originalTiltedLeft = HeroTiltedLeft.transform.rotation;
	backwardsTiltedLeft = originalTiltedRight;
	backwardsTiltedLeft.Set (0, 205, 15.0f, originalTiltedRight.w);
	backwardsTiltedRight = originalTiltedLeft;
	backwardsTiltedRight.z = 160.0f;
	/*
	originalPosition = GameObject.FindGameObjectWithTag ("Hero").transform.rotation;
	alternatePosition = originalPosition;
	alternatePosition.Set(originalPosition.x, originalPosition.y, -originalPosition.z, originalPosition.w);
	*/
		}
	
		void UpdateMovement ()
		{
				// Movement
				float z = -Input.GetAxis ("Horizontal");
				float x = Input.GetAxis ("Vertical");
				
				checkRunningAnimation (z, x);
						
					
				Vector3 inputVec;
				//	timeAtStartOfMovement = Time.time;
				if (touchingLadder) {
						inputVec = new Vector3 (0, 0, 0);
						Debug.Log ("Touching Ladder");
						if (x == 1) { // Going up
								if (!canJump && controller.isGrounded) { // Encontra-se no topo
										controller.Move (new Vector3 (0f, 0.5f, 0f));
								} else { // Encontra-se a meio da escada
										canJump = false;
										//	verticalVel += runSpeed;
										//	controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
										controller.Move (new Vector3 (0f, 0.09f, 0f));
								}
						} else if (x == -1) { // Going down
								if (!controller.isGrounded) {
										canJump = false;
										//	verticalVel -= runSpeed;
										//	controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
										controller.Move (new Vector3 (0f, -0.09f, 0f));
								} else { // Already on Floor
										canJump = true;
										touchingLadder = false;
										Debug.Log ("IS GROUNDED");
								}
						}
				} else {
						// Movimento 3D
						if (GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled) {
								inputVec = new Vector3 (x, 0, z) * runSpeed * 2;
	
								// Check Ladder
								{ // Movimento normal
										controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
	
										if (z == -1 && !tiltedRightIn3D) {
												if (facingBackwards)
														setHeroDirection (2);
												else
														setHeroDirection (0);
												//	GameObject.FindGameObjectWithTag ("Hero_Inverse").GetComponent
												//		GameObject.FindGameObjectWithTag ("Hero").SetActive(true);
										} else if (z == 1 && tiltedRightIn3D) {
												if (facingBackwards)
														setHeroDirection (3);
												else
														setHeroDirection (1);
										}
	
										if (x == -1 && !facingBackwards) {
												if (tiltedRightIn3D)
														setHeroDirection (2);
												else
														setHeroDirection (3);
										} else if (x == 1 && facingBackwards) {
												if (tiltedRightIn3D)
														setHeroDirection (0);
												else
														setHeroDirection (1);
										}
								}
						} else { // Movimento 2D
								if (z == -1) {
										facingRight = true;
										Hero2DInverse.SetActive (false);
										Hero2D.SetActive (true);
								} else if (z == 1) {
										facingRight = false;
										Hero2DInverse.SetActive (true);
										Hero2D.SetActive (false);
								}
								inputVec = new Vector3 (-z, 0, 0) * runSpeed * 2;
								controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
						}
				}
		}
		void Update ()
		{
				// Check for jump
				if (controller.isGrounded) {
						canJump = true;
						verticalVel = 0.0f;
						if (Input.GetKeyDown ("space")) {
								//	timeAtStartOfJump = Time.time; // Para gerir se o salto foi posterior a movimento
								// Apply the current movement to launch velocity
								verticalVel = jumpSpeed;
								canJump = false;
						}
				} else {
						// Apply gravity to our velocity to diminish it over time
						verticalVel += Physics.gravity.y * Time.deltaTime;
				}
	
				// Actually move the character
				UpdateMovement ();
	
				if (controller.isGrounded) {
						verticalVel = 0f;// Remove any persistent velocity after landing
				}
		}
	
		void OnTriggerEnter (Collider other)
		{
				if (other.tag == "Ladder") {
						touchingLadder = true;
				}
		}
	
		void OnTriggerExit (Collider other)
		{
				if (other.tag == "Ladder") {
						touchingLadder = false;
						Debug.Log ("EXIT LADDER");
				}
		}
	
		public void setHero2D (bool b)
		{
				if (b) {
						if (facingRight)
								Hero2D.SetActive (true);
						else
								Hero2DInverse.SetActive (true);
						HeroTiltedRight.SetActive (false);
						HeroTiltedLeft.SetActive (false);
				} else {
						Hero2D.SetActive (false);
						Hero2DInverse.SetActive (false);
						if (facingRight)
								setHeroDirection (0);
						else
								setHeroDirection (2);
				}
		}
	
		/**
	* @params: dir
	* 	0 = tilted right
	*  1 = tilted left
	* 	2 = tilted right (from camera perspective) backwards
	*  3 = tilted left (from camera perspective) backwards
	**/
		public void setHeroDirection (int dir)
		{
				Hero2D.SetActive (false);
				Hero2DInverse.SetActive (false);
	
				if (dir == 0) {
						tiltedRightIn3D = true;
						facingBackwards = false;
						HeroTiltedRight.SetActive (true);
						HeroTiltedLeft.SetActive (false);
						HeroTiltedRightBackwards.SetActive (false);
						HeroTiltedLeftBackwards.SetActive (false);
				} else if (dir == 1) {
						tiltedRightIn3D = false;
						facingBackwards = false;
						HeroTiltedRight.SetActive (false);
						HeroTiltedLeft.SetActive (true);
						HeroTiltedRightBackwards.SetActive (false);
						HeroTiltedLeftBackwards.SetActive (false);
				} else if (dir == 2) {
						facingBackwards = true;
						tiltedRightIn3D = true;
						HeroTiltedRight.SetActive (false);
						HeroTiltedLeft.SetActive (false);
						HeroTiltedRightBackwards.SetActive (true);
						HeroTiltedLeftBackwards.SetActive (false);
				} else if (dir == 3) {
						facingBackwards = true;
						tiltedRightIn3D = false;
						HeroTiltedRight.SetActive (false);
						HeroTiltedLeft.SetActive (false);
						HeroTiltedRightBackwards.SetActive (false);
						HeroTiltedLeftBackwards.SetActive (true);
				}
		}
		
		void checkRunningAnimation (float z, float x)
		{
				if (z == 1 || z == -1 || x == 1 || x == -1) {
						if (!animation.IsPlaying ("run"))
								animation.Play ("run");
				} else
						animation.Play ("idle");
		}
}