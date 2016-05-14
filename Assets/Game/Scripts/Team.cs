using UnityEngine;
using System.Collections.Generic;

public class Team : MonoBehaviour {
    
    public Material colorMaterial;
    public Transform spawnPoint;

    public int playerLimit = 1;
    public List<Player> players = new List<Player>();

    public int CurrentPlayerCount
    {
        get
        {
            return players.Count;
        }
    }

    public bool HasCapacity
    {
        get
        {
            return CurrentPlayerCount < playerLimit;
        }
    }

    public void Start()
    {
        TeamManager.Instance.AddTeam(this);
    }

    public Player AddPlayer(GameObject prefab, Transform spawnLocation)
    {
        GameObject go = (GameObject)Instantiate(prefab, spawnLocation.position, spawnLocation.rotation);
        Player p = go.GetComponent<Player>();

        if (HasCapacity && !players.Contains(p))
        {
            players.Add(p);
        }

        return p;
    }

}
