using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ConnectionManager : MonoBehaviour {

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
		if (AirConsole.instance.GetActivePlayerDeviceIds.Count < 4) {
			AirConsole.instance.SetActivePlayers (4);

			int playerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber (deviceId);

			// If our player is reconnecting then we do not need to respawn them a character because
			// their character already exists.
			if (!players.ContainsKey(playerNumber)) {
				GameObject go = (GameObject) Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
			 	Player p = go.GetComponent<Player> ();
				players.Add (playerNumber, p);
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

		if (playerNumber != -1) {
			Player p = players [playerNumber];
			p.ProcessMessage (data);
		}
	}
}
