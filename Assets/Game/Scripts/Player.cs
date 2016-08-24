using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

public class Player : MonoBehaviour {

    // Common Player Info
    public float speed = 10.0f;
    public int maxHealth = 100;
    public Transform gunTip;
    public GameObject standardBullet;

    // Components
    protected Rigidbody playerRigidBody;
    protected BoxCollider playerCollider;
    protected GameObject playerBody;

    // Player Information
    public int playerNumber;
	protected float health;
    protected Vector3 playerInput = Vector3.zero;

    // Spawn information
    protected GameObject bodyPrefab;
    protected Team team;


	public void Awake()
    {
		playerRigidBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
	}
        
    public void Update()
    {
        Vector3 rawVelocity = Vector3.ClampMagnitude((playerInput) * speed, speed);
        playerRigidBody.velocity = Vector3.Slerp(playerRigidBody.velocity, rawVelocity, Time.fixedDeltaTime * speed);

        if (playerInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerRigidBody.velocity);
            playerRigidBody.rotation = Quaternion.Slerp(playerRigidBody.rotation, targetRotation, Time.fixedDeltaTime * speed);
        }
    }
    
    public void ProcessMessage(JToken data) {
        try {
			if (data["Shoot"] != null) {
	            Shoot();
			}

            if (data["joystick-left"] != null)
            {
                if ((bool)data["joystick-left"]["pressed"]) {
                    float x = (float)data["joystick-left"]["message"]["x"];
                    float y = (float)data["joystick-left"]["message"]["y"];

                    playerInput = TranslateXYToVector(x, y);
                }
                else
                {
                    playerInput = Vector3.zero;
                }
            }
		} catch (Exception e) {
			Debug.LogError ("Exception handling player input: " + e.ToString());
		}
	}

    protected Vector3 TranslateXYToVector(float x, float y)
    {
        Vector3 temp = Vector3.zero;

        if (y < 0)
        {
            temp += Vector3.forward * Math.Abs(y);
        }
        else if (y > 0)
        {
            temp += Vector3.back * Math.Abs(y);
        }

        if (x > 0)
        {
            temp += Vector3.right * Math.Abs(x);
        }
        else if (x < 0)
        {
            temp += Vector3.left * Math.Abs(x);
        }

        return temp;
    }

    protected void Shoot()
    {
        GameObject go = (GameObject) Instantiate(standardBullet, gunTip.position, transform.rotation);

        StandardBullet bullet = go.GetComponent<StandardBullet>();
        bullet.Shooter = this; 
    }

    public void AssignPlayerToTeam(GameObject prefab, Team team)
    {
        bodyPrefab = prefab;
        this.team = team;
    }

	public void TakeDamage(float damage, Player shooter)
    {
		// prevent friendly fire
		if (team != shooter.team) {

			health -= damage;

			if (health <= 0) {
				shooter.team.HandlePlayerKill ();
				Die ();
			}
		}
    }

    public void Spawn()
    {
        playerBody = (GameObject)Instantiate(bodyPrefab);
        playerBody.GetComponent<MeshRenderer>().material = team.colorMaterial;
        playerBody.transform.SetParent(this.transform);
        playerBody.transform.localPosition = Vector3.zero;
        playerBody.transform.rotation = this.transform.rotation;

        this.transform.position = team.spawnPoint.position;
        this.transform.rotation = team.spawnPoint.rotation;

        health = maxHealth;
        playerCollider.isTrigger = false; // re-enable colliders so they can be crashed into
    }

    protected void Die()
    {
        Destroy(playerBody);
        team.HandlePlayerDeath(this);

        playerCollider.isTrigger = true; // since the player is dead, other players should be able to pass through them
        playerBody = null;
    }
}
