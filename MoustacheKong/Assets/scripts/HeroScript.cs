using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof (CharacterController))]
[AddComponentMenu("Third Person Player/Third Person Controller")]

public class HeroScript : MonoBehaviour {

	// Controlo das Ladders
	private bool touchingLadder = false;
	public float rotationDamping = 30f;
	public float runSpeed = 5f;
	public int gravity = 5;
	public float jumpSpeed = 10;

	// Controlo do personagem
	public bool rotatedRight = true;
	public GameObject Hero;
	public GameObject HeroInverse;
	//	private Quaternion originalPosition, alternatePosition;
	
	bool canJump;
	float moveSpeed;
	float verticalVel;  // Used for continuing momentum while in air    
	CharacterController controller;
	
	void Start()
	{
		controller = (CharacterController)GetComponent(typeof(CharacterController));

		/*
		originalPosition = GameObject.FindGameObjectWithTag ("Hero").transform.rotation;
		alternatePosition = originalPosition;
		alternatePosition.Set(originalPosition.x, originalPosition.y, -originalPosition.z, originalPosition.w);
		*/
	}

	void UpdateMovement()
	{
		// Movement
		float z = -Input.GetAxis("Horizontal");
		float x = Input.GetAxis("Vertical");
		// Movimento 3D
		if (GameObject.FindGameObjectWithTag ("Player").GetComponent<GameLogic> ().Camera3D.enabled) {
		
				Vector3 inputVec = new Vector3 (x, 0, z);
				inputVec *= runSpeed;

				// Check Ladder
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
				} else { // Movimento normal
						controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);

					if(z == -1 && !rotatedRight){
						rotatedRight = true;
						Hero.SetActive(true);
						HeroInverse.SetActive(false);
				//	GameObject.FindGameObjectWithTag ("Hero_Inverse").GetComponent
			//		GameObject.FindGameObjectWithTag ("Hero").SetActive(true);
					}
					else if (z == 1 && rotatedRight){
						rotatedRight = false;
						HeroInverse.SetActive(true);
						Hero.SetActive(false);
				}
						// Rotation
			/*			if (inputVec != Vector3.zero)
								transform.rotation = Quaternion.Slerp (transform.rotation, 
              Quaternion.LookRotation (inputVec), 
              Time.deltaTime * rotationDamping);
			*/	}
		} else { // Movimento 2D
			if(z == -1){
				Debug.Log (this.transform.position.x);
			//	this.transform.position = new Vector3(this.transform.position.x + 1, this.transform.position.y,
			//	                            this.transform.position.z);
				controller.SimpleMove(new Vector3(runSpeed*2, 0f, 0f));
			}
			else if(z == 1){
				Debug.Log ("LEFT");
				controller.SimpleMove(new Vector3(-runSpeed*2, 0f, 0f));
			}
		}
	}
	void Update()
	{
		// Check for jump
		if (controller.isGrounded )
		{
			canJump = true;
			if ( canJump && Input.GetKeyDown("space") )
			{
				// Apply the current movement to launch velocity
				verticalVel = jumpSpeed;
				canJump = false;
			}
		}else
		{           
			// Apply gravity to our velocity to diminish it over time
			verticalVel += Physics.gravity.y * Time.deltaTime;
		}
		
		// Actually move the character
	//	moveSpeed = UpdateMovement();  
		UpdateMovement();

		if ( controller.isGrounded )
			verticalVel = 0f;// Remove any persistent velocity after landing
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Ladder") {
			touchingLadder = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Ladder") {
			touchingLadder = false;
			Debug.Log ("EXIT LADDER");
		}
	}
}