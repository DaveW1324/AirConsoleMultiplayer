using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

public class Player : MonoBehaviour {

    // Common Player Info
    public float speed = 10.0f;
    public float timeToMaxSpeed = 0.5f;
    public float timerCooldown = 0f;

	// Components
	protected Rigidbody playerBody;

	// Player Information
	protected int health;

	public void Awake() {
		playerBody = GetComponent<Rigidbody> ();
	}
   
	public void ProcessMessage(JToken data) {
		try{
			Debug.Log(data.First);

			if (data["Shoot"] != null) {
				Debug.Log("Shooting");
				Debug.Log(data["Shoot"]["pressed"]);
			}			
			else if((bool) data["joystick-left"]["pressed"]) {
				float x = (float) data["joystick-left"]["message"]["x"];
				float y = (float) data["joystick-left"]["message"]["y"];

                Vector3 oldVelocity = playerBody.velocity;
                Vector3 newVelocity = new Vector3(x, 0, -y) * speed;


                playerBody.rotation = Quaternion.LookRotation(newVelocity);
                playerBody.velocity = newVelocity;

                /*
                timerCooldown = timerCooldown + Time.deltaTime > timerCooldown ? timerCooldown : timerCooldown + Time.deltaTime;                
                playerBody.rotation = Quaternion.Lerp(playerBody.rotation, Quaternion.LookRotation(newVelocity), timerCooldown);
                playerBody.velocity = Vector3.Lerp(oldVelocity, newVelocity, timerCooldown);
                */
            }
            else
            {
                playerBody.velocity = Vector3.zero;
            }
		} catch (Exception e) {
			Debug.LogError ("Exception handling player input: " + e.ToString());
		}
	}		
}
