using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {

    public float speed;
    public float damage;
    public float radius;
    public float duration;

    public void OnCollisionEnter(Collision collision) {
        //TODO: display the particle effect
        PerformDamage(collision);
        //Destroy(this.gameObject);
    }

    public abstract void PerformDamage(Collision collision);
}
