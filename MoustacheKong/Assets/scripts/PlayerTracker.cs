using UnityEngine;
using System.Collections;

public class PlayerTracker : MonoBehaviour {

	public GameObject player;

	public int dir = 1;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update (){

		HeroScript hs = (HeroScript) player.GetComponent<HeroScript>();
		// Get the current platform
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, 5)
		    && !hs.charOnLadder && !hs.touchingLadder) {
			if (hit.collider.gameObject.tag.Equals ("bck")) {
					dir = -1;
			} else if (hit.collider.gameObject.tag.Equals ("fwd")) {
					dir = 1;
			}
		}
		Vector3 pos = transform.position;
		//TODO: Get the direction from the current platform

		pos.x = player.transform.position.x - 6 * dir;
		pos.y = player.transform.position.y + 2;
		pos.z = player.transform.position.z;

		pos.x = Mathf.Clamp (pos.x, -22, 29);
		Quaternion rot = transform.rotation;
		rot = Quaternion.Euler (0, dir * 90, 0);

		transform.position = pos;
		transform.rotation = rot;

	}
}
