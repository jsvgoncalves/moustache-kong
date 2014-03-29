using UnityEngine;
using System.Collections;

public class BarrelScript : MonoBehaviour {
	
	
	Vector3 velocity = Vector3.zero;
	
	public Vector3 gravity;
	public Vector3 gravityX;
	public Vector3 jumpVelocity;
	public float maxSpeed = 5f;

	bool stupidFlag = false;
	
	// Used to calculate the movement direction
	private int dir = 1;
	
	
	// Use this for initialization
	void Start () {
		// Choose a random lane to start?
	}
	
	// Update is called once per frame
	void Update () {
		// stuff maybe?
	}
	
	void FixedUpdate() {

		// Destroy the barrel when it reaches 0.5y
		if(transform.position.y <= 0.5) {
			Destroy (gameObject);
		}
		velocity.y = 0;
		float y = transform.position.y;
		// Get the current platform
		RaycastHit hit;
		// The direction is actually the opposite of the platform tag. (Goes against the player)
		if (Physics.Raycast (transform.position, Vector3.down, out hit, 100)) {
			if (hit.collider.gameObject.tag.Equals ("bck") && hit.distance < 2) {
				dir = 1;
			} else if(hit.collider.gameObject.tag.Equals ("fwd") && hit.distance < 2) {
				dir = -1;
			}

			// FIXME: Come up with a better way of making the barrels' movement.
			// Check if the barrel is on the air or rolling through a platform
			if(hit.distance > 1 ) {
				velocity.y = gravity.y;
				stupidFlag = false;
			} else {
				y = transform.position.y + 0.5f - hit.distance;
				stupidFlag = true;
			}
		}
		
		velocity.x = dir * gravityX.x;
		velocity.z = 0;
		
		//		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
		Vector3 newPos = transform.position + velocity * Time.deltaTime;

		// If the barrel is on the air then let it have velocity;
		// If it grounded then the position should be the fixed value of the platform position.
		newPos.y = stupidFlag ? y : newPos.y;
		transform.position = newPos;
	}

//	void OnTriggerEnter (Collider buh) {
//		Debug.Log ("Buh");
//	}
}