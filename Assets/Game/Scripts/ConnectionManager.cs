﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ConnectionManager : MonoBehaviour {

    public bool testing = true;
    public int testingNumberOfPlayers = 4;
    public int testingPlayerNumber = 0;

    public Transform spawnLocation;
    public GameObject basePlayer;
    public GameObject basePlayerBody;

	protected Dictionary<int, Player> players = new Dictionary<int, Player> ();

	public void Awake() {
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

    public void Start()
    {
        if (testing)
        {
            StartCoroutine(WaitAndAddPlayers(testingNumberOfPlayers));
        }
    }

    public void Update()
    {
        if (testing)
        {
            Vector2 testInput = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                testInput += Vector2.down;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                testInput += Vector2.up;
            }

            if (Input.GetKey(KeyCode.A))
            {
                testInput += Vector2.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                testInput += Vector2.right;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                ProcessMessage(testingPlayerNumber, JTokenCreator.GetShootJToken());
            }

            ProcessMessage(testingPlayerNumber, JTokenCreator.GetControllerJToken(testInput));
        }
    }


	/// <summary>
	/// Raises the connect event.
	/// </summary>
	/// <param name="deviceId">Device identifier.</param>
	public void OnConnect (int deviceId)
	{
		Debug.LogWarning ("Player Connected: " + deviceId);

        if (TeamManager.Instance.HasCapacity())
        {
            int playerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber(deviceId);

            // If our player is reconnecting then we do not need to respawn them a character because
            // their character already exists.
            if (!players.ContainsKey(playerNumber))
            {
                GameObject go = (GameObject)Instantiate(basePlayer);
                Player p = go.GetComponent<Player>();
                TeamManager.Instance.AssignPlayerToRandomTeam(p, basePlayerBody);

                // Add 1 to the current number because we have to adjust to the fact that this player 
                // has not been added to our dictinoary yet
                AirConsole.instance.SetActivePlayers(players.Count + 1);

                // Regrab the player number in the event the player didnt exist yet then it will 
                // not have had a valid value (would have been -1)
                if(testing)
                {
                    playerNumber = deviceId;
                }
                else
                {
                    playerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber(deviceId);
                }

                // Add the player to our list with this new number and update our Player to be aware
                players.Add(deviceId, p);                
                p.playerNumber = playerNumber;                

                Debug.Log("Added DeviceId: " + deviceId + " as PlayerNumber: " + playerNumber);
                Debug.Log("Number of Players: " + players.Count);
            }
		} else {
			Debug.LogWarning ("Too many players");
		}			
	}

	/// <summary>
	/// If the game is running and one of our active players leaves we should do something
	/// </summary>
	/// <param name="deviceId">Device identifier.</param>
	public void OnDisconnect (int deviceId)
	{
		Debug.LogWarning ("Player Disconnected: " + deviceId);
	}


	/// <summary>
	/// Check which of the players has sent the message
	/// </summary>
	/// <param name="deviceId">DeviceId.</param>
	/// <param name="data">Data.</param>
	void OnMessage (int deviceId, JToken data) {
		int playerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber (deviceId);
        //Debug.Log("Received Message for DeviceID: " + deviceId + " which is PlayerNumber: " + playerNumber);
        ProcessMessage(deviceId, data);		
	}

    protected void ProcessMessage(int deviceId, JToken data)
    {
		if (players.ContainsKey(deviceId))
        {
			Player p = players[deviceId];
            p.ProcessMessage(data);
        }
        else
        {
			Debug.LogWarning("Attempted to Process a message from an invalid DeviceId: " + deviceId);
        }
    }

    protected IEnumerator WaitAndAddPlayers(int numberPlayers)
    {
        for (int i = 0; i < numberPlayers; i++)
        {
            yield return new WaitForSeconds(1.0f);
            OnConnect(i);
        }
    }
}
