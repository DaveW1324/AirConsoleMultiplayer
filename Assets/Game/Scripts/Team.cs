using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

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

    public void AddPlayer(Player player, GameObject prefab)
    {
        if (HasCapacity && !players.Contains(player))
        {
            players.Add(player);
            player.AssignPlayerToTeam(prefab, this);
            player.Spawn();
        }
    }

    public void HandlePlayerDeath(Player player)
    {
        StartCoroutine(WaitAndSpawn(player));
    }

    protected IEnumerator WaitAndSpawn(Player player)
    {
        yield return new WaitForSeconds(2f);
        player.Spawn();
    }
}
