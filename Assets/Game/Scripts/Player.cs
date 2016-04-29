using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

public class Player : MonoBehaviour {

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
				playerBody.velocity = new Vector3(x, 0, y) * 10;
			}
				

		} catch (Exception e) {
			Debug.LogError ("I am so lost");
		}
	}		
}
