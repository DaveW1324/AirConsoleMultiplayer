using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ConnectionManager : MonoBehaviour {

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

		int activePlayer = AirConsole.instance.ConvertDeviceIdToPlayerNumber (deviceId);

		if (activePlayer != -1) {
			Debug.LogWarning ("Player Disconnected: " + activePlayer);
		}
	}


	/// <summary>
	/// Check which of the players has sent the message
	/// </summary>
	/// <param name="deviceId">DeviceId.</param>
	/// <param name="data">Data.</param>
	void OnMessage (int deviceId, JToken data) {
		int activePlayer = AirConsole.instance.ConvertDeviceIdToPlayerNumber (deviceId);

		if (activePlayer != -1) {
			Debug.LogWarning ("Successfully recieved message from: " + activePlayer);
		}
	}
}
