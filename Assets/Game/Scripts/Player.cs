using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Components
	protected Rigidbody playerBody;

	// Player Information
	protected int health;

	public void Awake() {
		playerBody = GetComponent<Rigidbody> ();
	}

	public void Move(float move) {
		playerBody.velocity = Vector3.forward * (move * 10);
	}
}
