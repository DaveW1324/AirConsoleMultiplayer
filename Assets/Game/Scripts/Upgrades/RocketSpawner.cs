using UnityEngine;
using System.Collections;

public class RocketSpawner : MonoBehaviour {

	public GameObject rocketUpgrade;
	public float respawnRate = 15f;

	private float timer;
	private bool hasUpgrade = false;

	public void Start () {
		timer = respawnRate;
	}

	public void FixedUpdate()
	{
		if (timer > 0f) 
		{
			timer -= Time.fixedDeltaTime;

			if (timer <= 0f && !hasUpgrade) 
			{
				SpawnUpgrade ();
			}
		}
	}

	public void OnTriggerEnter(Collider collider)
	{
		if (hasUpgrade && collider.gameObject.GetComponent<Player> () != null) {
			hasUpgrade = false;
			timer = respawnRate;
		}
	}

	private void SpawnUpgrade()
	{
		Instantiate(rocketUpgrade, transform.position, transform.rotation);
		hasUpgrade = true;
	}
}
