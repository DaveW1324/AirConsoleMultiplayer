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

    public int maxNumberOfLives = 3;
    protected int livesRemaining;

    public List<Player> players = new List<Player>();

	public int score = 0;
    public TextMesh textMesh;

    protected bool stillAlive = true;

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

    public bool StillAlive
    {
        get
        {
            return stillAlive;
        }
    }

    public void Start()
    {
        TeamManager.Instance.AddTeam(this);
        livesRemaining = maxNumberOfLives;
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

	public void HandlePlayerKill()
	{
		score++;
        textMesh.text = score.ToString();
	}

    public void HandlePlayerDeath(Player player)
    {
        livesRemaining--;

        if (livesRemaining > 0)
        {
            StartCoroutine(WaitAndSpawn(player));
        }
        else
        {
            stillAlive = false;
        }
    }

    protected IEnumerator WaitAndSpawn(Player player)
    {
        yield return new WaitForSeconds(2f);
        player.Spawn();
    }
}
