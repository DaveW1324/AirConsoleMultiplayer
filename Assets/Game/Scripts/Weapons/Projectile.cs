using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {

    public float speed;
    public float damage;
    public float radius;
    public float duration;

	public GameObject particleExplosion;
	public float particleDuration = 0.25f;

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

		if (collision.gameObject.GetComponent<Player> () == shooter) {
			return;
		}

		DisplayParticleEffect ();
		PerformDamage(collision);
        Destroy(this.gameObject);
    }

	public void DisplayParticleEffect()
	{
		GameObject ps = Instantiate(particleExplosion, transform.position, transform.rotation) as GameObject;
		Destroy (ps, particleDuration);
	}

    public abstract void PerformDamage(Collision collision);
}
