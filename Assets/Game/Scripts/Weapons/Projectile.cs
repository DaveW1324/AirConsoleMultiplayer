using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {

    public float speed;
    public float damage;
    public float radius;
    public float duration;

    protected Player shooter;

    public Player Shooter
    {
        get
        {
            return shooter;
        }

        set 
        {
            shooter = value;
        }
    }

    public void OnCollisionEnter(Collision collision) {
        //TODO: display the particle effect
        PerformDamage(collision);
        //Destroy(this.gameObject);
    }

    public abstract void PerformDamage(Collision collision);
}
