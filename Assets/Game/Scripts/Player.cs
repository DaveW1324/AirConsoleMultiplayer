using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

public class Player : MonoBehaviour {

    // Common Player Info
    public float speed = 10.0f;
    public Transform gunTip;
    public GameObject standardBullet;

    // Components
    protected Rigidbody playerRigidBody;
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
		playerRigidBody = GetComponent<Rigidbody> ();
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

        Debug.Log(data);

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
        bodyPrefab.gameObject.GetComponent<MeshRenderer>().material = team.colorMaterial;
        this.team = team;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Spawn()
    {
        playerBody = (GameObject)Instantiate(bodyPrefab);
        playerBody.transform.SetParent(this.transform);

        this.transform.position = team.spawnPoint.position;
        this.transform.rotation = team.spawnPoint.rotation;
    }

    protected void Die()
    {
        Destroy(playerBody);
        team.HandlePlayerDeath(this);
        playerBody = null;
    }
}
