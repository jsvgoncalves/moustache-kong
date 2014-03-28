using UnityEngine;
using System.Collections;

public class BarrelScript : MonoBehaviour {


	Vector3 velocity = Vector3.zero;

	public Vector3 gravity;
	public Vector3 gravityX;
	public Vector3 jumpVelocity;
	public float maxSpeed = 5f;

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
		velocity.y = 0;

		// Get the current platform
		RaycastHit hit;
		// The direction is actually the opposite of the platform tag. (Goes against the player)
		if (Physics.Raycast (transform.position, Vector3.down, out hit, 100)) {
			if (hit.collider.gameObject.tag.Equals ("bck") && hit.distance < 2) {
				dir = 1;
			} else if(hit.collider.gameObject.tag.Equals ("fwd") && hit.distance < 2) {
				dir = -1;
			}


			if(hit.distance > 0.5 ) {
				velocity.y = gravity.y;
			}

//			Debug.Log(hit.distance);
//			Debug.Log(hit.collider.gameObject.tag);
		}

		velocity.x = dir * gravityX.x;
		velocity.z = 0;

//		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
		
		transform.position += velocity * Time.deltaTime;
	}
}
