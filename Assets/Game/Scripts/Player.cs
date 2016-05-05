using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

public class Player : MonoBehaviour {

    // Common Player Info
    public float speed = 10.0f;

    // Components
    public GameObject avatar;
    protected Rigidbody playerRigidBody;

	// Player Information
	protected int health;
    protected Vector3 playerInput = Vector3.zero;

	public void Awake()
    {
		playerRigidBody = GetComponent<Rigidbody> ();
	}

    /*
    // Should be used for testing only
    public void Update()
    {
        Vector3 testInput = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            testInput += Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            testInput += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            testInput += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            testInput += Vector3.right;
        }

        playerInput = testInput;
    }
    */

    public void FixedUpdate()
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
				Debug.Log("Shooting");
				Debug.Log(data["Shoot"]["pressed"]);
			}
            			
			if((bool) data["joystick-left"]["pressed"]) {
				float x = (float) data["joystick-left"]["message"]["x"];
				float y = (float) data["joystick-left"]["message"]["y"];

                playerInput = TranslateXYToVector(x, y);
            }
            else
            {
                playerInput = Vector3.zero;
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
            temp += Vector3.forward;
        }
        else if (y > 0)
        {
            temp += Vector3.back;
        }

        if (x > 0)
        {
            temp += Vector3.right;
        }
        else if (x < 0)
        {
            temp += Vector3.left;
        }

        return temp;
    }
}
