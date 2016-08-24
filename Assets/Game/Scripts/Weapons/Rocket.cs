using UnityEngine;
using System.Collections;
using System;

public class Rocket : Projectile
{
    public void Start()
    {
        Destroy(this.gameObject, duration);
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
            Debug.Log("Hit Collider: " + hitColliders[i]);

            Player p = hitColliders[i].gameObject.GetComponent<Player>();

            if (p != null)
            {
                //TODO: do some sort of offset depending on closeness
                float damageAmt = damage;
				p.TakeDamage(damageAmt, shooter);
            }
        }
    }
}
