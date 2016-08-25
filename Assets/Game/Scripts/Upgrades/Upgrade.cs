using UnityEngine;
using System.Collections;

public abstract class Upgrade : MonoBehaviour {

	public GameObject prefab;
	public int numberOfShots = 5;

	protected int shotsTaken = 0;

	public abstract void OnTriggerEnter (Collider collider);

	/**
	 * Returns true if powerup remains, returns false if powerup should be removed
	 */
	public abstract bool Shoot (Player shooter, Vector3 shootPosition, Quaternion shootRotation);
}
