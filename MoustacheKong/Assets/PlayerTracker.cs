using UnityEngine;
using System.Collections;

public class PlayerTracker : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		//TODO: Get the direction from the current platform
		int dir = 1;
		pos.x = player.transform.position.x + 5 * dir;
		pos.y = player.transform.position.y + 2;
		pos.z = player.transform.position.z;

		transform.position = pos;
	}
}
