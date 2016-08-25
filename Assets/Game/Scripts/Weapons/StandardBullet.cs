using UnityEngine;
using System.Collections;
using System;

public class StandardBullet : Projectile
{
    public void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(this.gameObject, duration);
    }

    public override void PerformDamage(Collision collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();
        
        if (p != null)
        {
            p.TakeDamage(damage, shooter);
        }
    }
}
