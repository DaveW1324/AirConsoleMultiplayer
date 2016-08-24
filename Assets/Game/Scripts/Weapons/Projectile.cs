using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {

    public float speed;
    public float damage;
    public float radius;
    public float duration;

	public GameObject particleSystem;

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
		GameObject ps = Instantiate(particleSystem, transform.position, transform.rotation) as GameObject;
		Destroy (ps, .25f);

        PerformDamage(collision);
        Destroy(this.gameObject);
    }

    public abstract void PerformDamage(Collision collision);
}
