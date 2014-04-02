using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	
	public LayerMask collisionMask;

	private BoxCollider collider;
	private Vector3 s;
	private Vector3 c;
	
	private float skin = .005f;
	
	[HideInInspector]
	public bool grounded;
	
	Ray ray;
	RaycastHit hit;
	
	void Start() {
		collider = GetComponent<BoxCollider>();
		s = collider.size;
		c = collider.center;
	}

	public void Move(Vector2 moveAmount) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position;
		
		// Check collisions below
		grounded = false;

		float dirX = Mathf.Sign(deltaX);
		float lastDir = dirX;
		if (dirX != 0 && lastDir != dirX) {
			lastDir = dirX;
			Vector3 theScale = transform.localScale;
			theScale.x *= dirX;
			transform.localScale = theScale;
		}

		float dir = Mathf.Sign(deltaY);
		if (dir == 0 || dir == -1) {
			dir = -1;

			for (int i = 0; i<3; i ++) {
				float x = (p.x + c.x - s.x/2) + s.x/2 * i; // left, centre and then rightmost point of collider
				float y = p.y + c.y - s.y/4; // bottom of collider
				
				
				ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
				Debug.DrawRay(ray.origin,ray.direction);
				if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaY) + s.y/4 + skin,collisionMask)) {
					// distance between player and ground
					float dst = Vector3.Distance (ray.origin, hit.point);
					
					//inside the body (no collision)
					if (dst < s.y/4) {
						deltaY = (- dst + s.y/4) * (-dir) + skin;
					} else {
						// stop player's downwards movement
						if (dst - s.y/4 > skin) {
							deltaY = (dst - s.y/4) * dir + skin;
						}
						else {
							deltaY = 0;
						}
					}

					grounded = true;
					break;
				}
			}
		}
				
		Vector2 finalTransform = new Vector2(deltaX,deltaY);
		transform.Translate(finalTransform);
	}
}
