using UnityEngine;
using System.Collections;
using System;

public class StandardBullet : Projectile
{
    public void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 20.0f;
        Destroy(this.gameObject, duration);
    }

    public override void PerformDamage(Collision collision)
    {
        //Player p = collision.gameObject.GetComponent<Player>();
        //p.TakeDamage(damage);
    }
}
