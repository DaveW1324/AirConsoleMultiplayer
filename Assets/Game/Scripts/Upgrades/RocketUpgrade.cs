using UnityEngine;
using System.Collections;

public class RocketUpgrade : Upgrade {

	public override void OnCollisionEnter (Collision collision)
	{
		Player p = collision.gameObject.GetComponent<Player> ();

		if (p != null) {
			p.currentUpgrade = this;
		}

		this.GetComponent<MeshRenderer> ().enabled = false;
		this.GetComponent<Collider> ().enabled = false;
	}


	public override bool Shoot(Player shooter, Vector3 shootPosition, Quaternion shootRotation)
	{
		GameObject go = (GameObject)Instantiate(prefab, shootPosition, shootRotation);

		Projectile p = go.GetComponent<Projectile> ();
		p.Shooter = shooter;

		shotsTaken++;
		return shotsTaken < numberOfShots;
	}
}
