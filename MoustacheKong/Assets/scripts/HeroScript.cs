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
	
	bool canJump;
	float moveSpeed;
	float verticalVel;  // Used for continuing momentum while in air    
	CharacterController controller;
	
	void Start()
	{
		controller = (CharacterController)GetComponent(typeof(CharacterController));
	}
	void UpdateMovement()
	{

		// Movement
		float z = -Input.GetAxis("Horizontal");
		float x = Input.GetAxis("Vertical");
		
		Vector3 inputVec = new Vector3(x, 0, z);
		inputVec *= runSpeed;

		// Check Ladder
		if (touchingLadder) {
				inputVec = new Vector3 (0, 0, 0);
				Debug.Log ("Touching Ladder");
				if (x == 1) { // Going up
					if(!canJump && controller.isGrounded){ // Encontra-se no topo
						controller.Move(new Vector3(0f, 0.5f, 0f));
					}
					else{ // Encontra-se a meio da escada
						canJump = false;
						//	verticalVel += runSpeed;
						//	controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
						controller.Move(new Vector3(0f, 0.09f, 0f));
					}
			} else if (x == -1) { // Going down
				if (!controller.isGrounded) {
						canJump = false;
					//	verticalVel -= runSpeed;
					//	controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);
						controller.Move(new Vector3(0f, -0.09f, 0f));		
				} else { // Already on Floor
						canJump = true;
						touchingLadder = false;
						Debug.Log ("IS GROUNDED");
					}
				}
		} 
		else {
				controller.Move ((inputVec + Vector3.up * -gravity + new Vector3 (0, verticalVel, 0)) * Time.deltaTime);

				// Rotation
				if (inputVec != Vector3.zero)
						transform.rotation = Quaternion.Slerp (transform.rotation, 
	                                      Quaternion.LookRotation (inputVec), 
	                                      Time.deltaTime * rotationDamping);
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