using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ConnectionManager : MonoBehaviour {

    public Transform spawnLocation;
    public GameObject playerPrefab;

	protected Dictionary<int, Player> players = new Dictionary<int, Player> ();

	public void Awake() {
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
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
                Player p = TeamManager.Instance.GetPlayerAssignedToRandomTeam(playerPrefab);

                // Add 1 to the current number because we have to adjust to the fact that this player 
                // has not been added to our dictinoary yet
                AirConsole.instance.SetActivePlayers(players.Count + 1);

                // Regrab the player number in the event the player didnt exist yet then it will 
                // not have had a valid value (would have been -1)
                playerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber(deviceId);

                // Add the player to our list with this new number and update our Player to be aware
                players.Add(playerNumber, p);                
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
		Debug.LogWarning ("OnDisconnect");

		int playerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber (deviceId);

		if (playerNumber != -1) {
			Debug.LogWarning ("Player Disconnected: " + playerNumber);
		}
	}


	/// <summary>
	/// Check which of the players has sent the message
	/// </summary>
	/// <param name="deviceId">DeviceId.</param>
	/// <param name="data">Data.</param>
	void OnMessage (int deviceId, JToken data) {
		int playerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber (deviceId);

        Debug.Log("Received Message for DeviceID: " + deviceId + " which is PlayerNumber: " + playerNumber);

		if (playerNumber != -1) {
			Player p = players [playerNumber];
			p.ProcessMessage (data);
		}
	}
}
