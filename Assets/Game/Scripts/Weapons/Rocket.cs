using UnityEngine;
using System.Collections;
using System;

public class Rocket : Projectile
{

	public int numberOfBullets = 5;

    public void Start()
    {
		this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;
		StartCoroutine(TimedExplosion());
    }

    public void OnDestroy()
    {
        PerformDamage(null);
    }

    public override void PerformDamage(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        for(int i = 0; i < hitColliders.Length; i++)
        {
            Player p = hitColliders[i].gameObject.GetComponent<Player>();

            if (p != null)
            {
                //TODO: do some sort of offset depending on closeness
                float damageAmt = damage;
				p.TakeDamage(damageAmt, shooter);
            }
        }
    }

	IEnumerator TimedExplosion() {
		yield return new WaitForSeconds(duration);
		GameObject ps = Instantiate(particleExplosion, transform.position, transform.rotation) as GameObject;
		Destroy (ps, particleDuration);
		PerformDamage (null);
		Destroy(this.gameObject);
	}
}
