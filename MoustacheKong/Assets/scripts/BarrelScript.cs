using UnityEngine;
using System.Collections;

public class BarrelScript : MonoBehaviour
{
	
	
		Vector3 velocity = Vector3.zero;
	
		public Vector3 gravity;
		public Vector3 gravityX;
		public Vector3 jumpVelocity;
		public float maxSpeed = 5f;

		public GameObject player;

		bool stupidFlag = false;
	
		// Used to calculate the movement direction
		private int dir = 1;
	
	
		// Use this for initialization
		void Start ()
		{
				// Choose a random lane to start?
		}

		/// <summary>
		/// Sets the lane.
		/// </summary>
		/// <param name="lane">Lane.</param>
		void setLane (int lane)
		{
				Vector3 newPosition = transform.position;
				//TODO : Set the appropriate coordinates.
				if (lane == 1) {
						newPosition.z = -3.12f;
				} else if (lane == 2) {
						newPosition.z = 0.25f;
				} else if (lane == 3) {
						newPosition.z = 3.58f;
				}
				transform.position = newPosition;
		}
	
		// Update is called once per frame
		void Update ()
		{
		}
	
		void FixedUpdate ()
		{
				GameObject[] g = GameObject.FindGameObjectsWithTag ("BarrelThing");
		
				for (int i = 0; i < g.Length; i++)
						g [i].transform.RotateAround (g [i].transform.parent.position, Vector3.forward, -dir * 600 * Time.deltaTime);
		
				velocity.y = 0;
				float y = transform.position.y;
				// Get the current platform
				RaycastHit hit;
				// The direction is actually the opposite of the platform tag. (Goes against the player)
				if (Physics.Raycast (transform.position, Vector3.down, out hit, 100)) {
						if (hit.collider.gameObject.tag.Equals ("bck") && hit.distance < 2) {
								dir = 1;
						} else if (hit.collider.gameObject.tag.Equals ("fwd") && hit.distance < 2) {
								dir = -1;
						}

						// FIXME: Come up with a better way of making the barrels' movement.
						// Only adjust if the raycast was on the platforms
						if (hit.collider.gameObject.tag.Equals ("fwd") || hit.collider.gameObject.tag.Equals ("bck")) {
								// Check if the barrel is on the air or rolling through a platform
								if (hit.distance > 1) {
										velocity.y = gravity.y;
										stupidFlag = false;
								} else {
										// Update the coords to make the barrel on top of the platform
										y = transform.position.y + 0.7f - hit.distance;
										stupidFlag = true;
								}
						}
				}
		
				velocity.x = dir * gravityX.x;
				velocity.z = 0;
		
				//		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
				Vector3 newPos = transform.position + velocity * Time.deltaTime;

				// If the barrel is on the air then let it have velocity;
				// If it is grounded then the position should be the fixed value of the platform position.
				newPos.y = stupidFlag ? y : newPos.y;
				transform.position = newPos;
		}

		/**
	 * Barrel is destroyed on Trigger
	 */
		void OnTriggerEnter (Collider other)
		{
				// Destroy the barrel when it reaches 0.5y
				if (other.tag == "Barrels_Death") {
						Destroy (gameObject);
				}
//		Debug.Log ("Buh");
		}

		void OnTriggerExit (Collider other)
		{
//		Debug.Log ("EXIT LADDER");
		}
}