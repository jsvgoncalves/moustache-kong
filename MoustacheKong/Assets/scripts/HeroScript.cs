﻿	using UnityEngine;
using System.Collections;
	
// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Third Person Player/Third Person Controller")]
	
public class HeroScript : MonoBehaviour
{

		public int score = 0;
		public int life = 3;
		public TextMesh scoreObj;

		// Controlo das Ladders
		public bool touchingLadder = false;
		public float rotationDamping = 30f;
		public float runSpeed = 5f;
		public int gravity = 15;
		public float jumpSpeed = 20;
		private bool lastLadderMovementUp = false;
		public bool charOnLadder = false;

		// Controlo do personagem
		public bool tiltedtoCamIn3D = true;
		public GameObject Hero2D, Hero2DInverse;
		public GameObject HeroTiltedRight, HeroTiltedRightBackwards, HeroTiltedLeft, HeroTiltedLeftBackwards, 
				InvertedDragonTiltedLeft, InvertedDragonTiltedRight, InvertedDragonTiltedLeftBackwards, InvertedDragonTiltedRightBackwards;
		public bool facingRight = true, facingBackwards = false;
		private int heroDirection = 0;
		private Vector3 original3DPosition;
		// Variaveis que guardam as posicoes (rotacoes) originais do Hero em 3D
		//	private Quaternion originalTiltedRight, originalTiltedLeft, backwardsTiltedLeft, backwardsTiltedRight;
		//	private float timeAtStartOfJump = 0.0f, timeAtStartOfMovement = 0.0f;
		//	private Quaternion originalPosition, alternatePosition;

		bool canJump = false;
		bool isStop = true;
		public AudioClip jumpClip;
		public AudioClip gameOverClip;
		private bool playingGameOver = false;
		private bool playingGameWin = false;
		public AudioClip ouchClip;
		public AudioClip scoreClip;
		public AudioClip gameWinClip;
		public GameObject gameOverGUI;
		public GameObject gameWinGUI;

		float moveSpeed;
		float verticalVel;  // Used for continuing momentum while in air
		CharacterController controller;
	
		void Start ()
		{
				controller = (CharacterController)GetComponent (typeof(CharacterController));
				original3DPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;

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
				// Se esta nas escadas trata o input de outra forma
				if (touchingLadder && x == 1) {
						if (!charOnLadder && x == 1) { // Se ainda nao estiver a subir e pressionar "Up"
								charOnLadder = true;
						} else { // Se ja estiver a subir
								inputVec = new Vector3 (0, 0, 0);
								Debug.Log ("Touching Ladder");
								if (x == 1) { // Going up
										lastLadderMovementUp = true;
										if (!canJump && controller.isGrounded) { // Encontra-se no topo
												controller.Move ((new Vector3 (0f, 5f, 50f) + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
										} else { // Encontra-se a meio da escada
												canJump = false;
												//	verticalVel += runSpeed;
												//	controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
												controller.Move (new Vector3 (0f, 0.09f, 0f));
										}
								} else if (x == -1) { // Going down
										lastLadderMovementUp = false;
										if (!controller.isGrounded) {
												canJump = false;
												//	verticalVel -= runSpeed;
												//	controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
												controller.Move (new Vector3 (0f, -0.09f, 0f));
										} else { // Already on Floor
												canJump = true;
												touchingLadder = false;
												charOnLadder = false;
												Debug.Log ("IS GROUNDED");
										}
								}
						}
				} else {
						// Movimento 3D
						if (GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled) {
								// Verifica se a posiçao e invertida
								if (GameObject.FindGameObjectWithTag ("Player").
								GetComponent<GameLogic> ().Camera3D.GetComponent<PlayerTracker> ().dir == -1) {
										x *= -1;
										z *= -1;
								}
								
								inputVec = new Vector3 (x, 0, z) * runSpeed * 2;
	
								// Check Ladder
								{ // Movimento normal
										controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
	
										if (z == -1 && !tiltedtoCamIn3D) {
												if (facingBackwards)
														setHeroDirection (2);
												else
														setHeroDirection (0);
												//	GameObject.FindGameObjectWithTag ("Hero_Inverse").GetComponent
												//		GameObject.FindGameObjectWithTag ("Hero").SetActive(true);
										} else if (z == 1 && tiltedtoCamIn3D) {
												if (facingBackwards)
														setHeroDirection (3);
												else
														setHeroDirection (1);
										}
	
										if (x == -1 && !facingBackwards) {
												if (tiltedtoCamIn3D)
														setHeroDirection (2);
												else
														setHeroDirection (3);
										} else if (x == 1 && facingBackwards) {
												if (tiltedtoCamIn3D)
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
								AudioSource.PlayClipAtPoint (jumpClip, transform.position);
								verticalVel = jumpSpeed;
								canJump = false;
						}
				} else {
						// Apply gravity to our velocity to diminish it over time
						verticalVel += Physics.gravity.y * Time.deltaTime * 1.1f;
				}
	
				// Actually move the character
				UpdateMovement ();
	
				if (controller.isGrounded) {
						verticalVel = 0f;// Remove any persistent velocity after landing
				}
		}
	
		void OnTriggerEnter (Collider other)
		{
				Debug.Log (other.tag);
				if (other.tag == "Ladder" && GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled) {
						touchingLadder = true;
				} else if (other.tag == "Ladder2D" && !GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled) {
						touchingLadder = true;
				} else if (other.tag == "EndPlatform") {
						gameWinGUI.SetActive(true);
						StartCoroutine (playSoundThenLoad (1));
				}
		}

		IEnumerator playSoundThenLoad (int sound)
		{
				if (!playingGameOver && !playingGameWin) {
						if (sound == 1) {
								AudioSource.PlayClipAtPoint (gameWinClip, transform.position, 1.0f);
								playingGameWin = true;
						} else {
								AudioSource.PlayClipAtPoint (gameOverClip, transform.position, 1.0f);
								playingGameOver = true;
						}
				}
				if (sound == 1)
						yield return new WaitForSeconds (gameWinClip.length + 1);
				else
						yield return new WaitForSeconds (gameOverClip.length + 1);
				Application.LoadLevel ("GUI");
		}

		void OnTriggerExit (Collider other)
		{
				touchingLadder = false;
				if (charOnLadder && (other.tag == "Ladder" || other.tag == "Ladder2D")) {
						charOnLadder = false;
						Debug.Log ("EXIT LADDER");
					
						if (lastLadderMovementUp) {
								if (GameObject.FindGameObjectWithTag ("Player").
							    	GetComponent<GameLogic> ().Camera3D.enabled && GameObject.FindGameObjectWithTag ("Player").
							   					GetComponent<GameLogic> ().Camera3D.GetComponent<PlayerTracker> ().dir == 1) {
										controller.Move (new Vector3 (-50f, 100f, 0f) * Time.deltaTime);
								} else if (GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled) {
										controller.Move (new Vector3 (50f, 100f, 0f) * Time.deltaTime);
								} else { // 2D Movement
										if (facingRight) {
												controller.Move (new Vector3 (50f, 100f, 0f) * Time.deltaTime);
										} else {
												controller.Move (new Vector3 (-50f, 100f, 0f) * Time.deltaTime);
										}
								}
						}
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
						HeroTiltedRightBackwards.SetActive (false);
						HeroTiltedLeftBackwards.SetActive (false);
						
						InvertedDragonTiltedLeft.SetActive (false);
						InvertedDragonTiltedRight.SetActive (false);
						InvertedDragonTiltedLeftBackwards.SetActive (false);
						InvertedDragonTiltedRightBackwards.SetActive (false);
				} else {
						GameObject.FindGameObjectWithTag ("Player").transform.position = original3DPosition;
					
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
	* 	0 = tilted right (or left in inverse)
	*  1 = tilted left
	* 	2 = tilted right (from camera perspective) backwards
	*  3 = tilted left (from camera perspective) backwards
	**/
		public void setHeroDirection (int dir)
		{
				Hero2D.SetActive (false);
				Hero2DInverse.SetActive (false);
								
				if (GameObject.FindGameObjectWithTag ("Player").
				    GetComponent<GameLogic> ().Camera3D.GetComponent<PlayerTracker> ().dir == 1) {
					
						if (dir == 0) {
								tiltedtoCamIn3D = true;
								facingBackwards = false;
								HeroTiltedRight.SetActive (true);
								HeroTiltedLeft.SetActive (false);
								HeroTiltedRightBackwards.SetActive (false);
								HeroTiltedLeftBackwards.SetActive (false);
						} else if (dir == 1) {
								tiltedtoCamIn3D = false;
								facingBackwards = false;
								HeroTiltedRight.SetActive (false);
								HeroTiltedLeft.SetActive (true);
								HeroTiltedRightBackwards.SetActive (false);
								HeroTiltedLeftBackwards.SetActive (false);
						} else if (dir == 2) {
								facingBackwards = true;
								tiltedtoCamIn3D = true;
								HeroTiltedRight.SetActive (false);
								HeroTiltedLeft.SetActive (false);
								HeroTiltedRightBackwards.SetActive (true);
								HeroTiltedLeftBackwards.SetActive (false);
						} else if (dir == 3) {
								facingBackwards = true;
								tiltedtoCamIn3D = false;
								HeroTiltedRight.SetActive (false);
								HeroTiltedLeft.SetActive (false);
								HeroTiltedRightBackwards.SetActive (false);
								HeroTiltedLeftBackwards.SetActive (true);
						}
				} else if (GameObject.FindGameObjectWithTag ("Player").
				             GetComponent<GameLogic> ().Camera3D.GetComponent<PlayerTracker> ().dir == -1) {
						if (dir == 0) {
								tiltedtoCamIn3D = true;
								facingBackwards = false;
								
								InvertedDragonTiltedLeft.SetActive (false);
								InvertedDragonTiltedRight.SetActive (false);
								InvertedDragonTiltedLeftBackwards.SetActive (true);
								InvertedDragonTiltedRightBackwards.SetActive (false);
								
								HeroTiltedRight.SetActive (false);
								HeroTiltedLeft.SetActive (false);
								HeroTiltedRightBackwards.SetActive (false);
								HeroTiltedLeftBackwards.SetActive (false);
						} else if (dir == 1) {
								tiltedtoCamIn3D = false;
								facingBackwards = false;
								
								InvertedDragonTiltedLeft.SetActive (false);
								InvertedDragonTiltedRight.SetActive (false);
								InvertedDragonTiltedLeftBackwards.SetActive (false);
								InvertedDragonTiltedRightBackwards.SetActive (true);
				
								HeroTiltedRight.SetActive (false);
								HeroTiltedLeft.SetActive (false);
								HeroTiltedRightBackwards.SetActive (false);
								HeroTiltedLeftBackwards.SetActive (false);
						} else if (dir == 2) {
								facingBackwards = true;
								tiltedtoCamIn3D = true;
								
								InvertedDragonTiltedLeft.SetActive (true);
								InvertedDragonTiltedRight.SetActive (false);
								InvertedDragonTiltedLeftBackwards.SetActive (false);
								InvertedDragonTiltedRightBackwards.SetActive (false);
				
								HeroTiltedRight.SetActive (false);
								HeroTiltedLeft.SetActive (false);
								HeroTiltedRightBackwards.SetActive (false);
								HeroTiltedLeftBackwards.SetActive (false);
						} else if (dir == 3) {
								facingBackwards = true;
								tiltedtoCamIn3D = false;
								
								InvertedDragonTiltedLeft.SetActive (false);
								InvertedDragonTiltedRight.SetActive (true);
								InvertedDragonTiltedLeftBackwards.SetActive (false);
								InvertedDragonTiltedRightBackwards.SetActive (false);
				
								HeroTiltedRight.SetActive (false);
								HeroTiltedLeft.SetActive (false);
								HeroTiltedRightBackwards.SetActive (false);
								HeroTiltedLeftBackwards.SetActive (false);
						}
				}
		}
		
		void checkRunningAnimation (float z, float x)
		{
				if (z == 1 || z == -1 || x == 1 || x == -1) {
						isStop = false;
						if (!animation.IsPlaying ("run"))
								animation.Play ("run");
				} else {
						animation.Play ("idle");
						isStop = true;
				}
		}
		
		public void set2DPositionZ ()
		{
				original3DPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
				GameObject.FindGameObjectWithTag ("Player").transform.position = new Vector3 (original3DPosition.x, original3DPosition.y, 3.0f);
		}
	
		public void setBackup3DPositionZ ()
		{
				Vector3 pos = GameObject.FindGameObjectWithTag ("Player").transform.position;
				GameObject.FindGameObjectWithTag ("Player").transform.position = new Vector3 (pos.x, 
		                                   pos.y, original3DPosition.z);
		}

		void barrelHit (int hit)
		{
				if (GameObject.FindGameObjectWithTag ("Player").
		    GetComponent<GameLogic> ().Camera3D.enabled && GameObject.FindGameObjectWithTag ("Player").
		    GetComponent<GameLogic> ().Camera3D.GetComponent<PlayerTracker> ().dir == 1) {
						controller.Move (new Vector3 (-10f, 80f, 0f) * Time.deltaTime);
				} else if (GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled) {
						controller.Move (new Vector3 (10f, 80f, 0f) * Time.deltaTime);
				} else { // 2D Movement
						if (facingRight) {
								controller.Move (new Vector3 (10f, 80f, 0f) * Time.deltaTime);
						} else {
								controller.Move (new Vector3 (-10f, 80f, 0f) * Time.deltaTime);
						}
				}
		
				if (life > 1)
						AudioSource.PlayClipAtPoint (ouchClip, transform.position, 1.0f);
				life -= 1;
				Debug.Log ("life: " + life);
				if (life <= 0) {
						gameOverGUI.SetActive(true);
						StartCoroutine (playSoundThenLoad (0));
				} else if (life == 1) {
						GameObject.Find ("Life2").SetActive (false);
				} else if (life == 2) {
						GameObject.Find ("Life1").SetActive (false);
						//FIXME: Reload level with less life.
//				Application.LoadLevel("Level1");
				} else if (life == 3) {
						GameObject.Find ("Life1").SetActive (true);
						GameObject.Find ("Life2").SetActive (true);
						GameObject.Find ("Life3").SetActive (true);
				
				}
		}

		/// <summary>
		/// Jumpeds the barrel.
		/// </summary>
		/// <param name="score">Score.</param>
		void jumpedBarrel (int score)
		{
				AudioSource.PlayClipAtPoint (scoreClip, transform.position, 1.0f);
				this.score += score;
				scoreObj.text = this.score + "";
		}
}