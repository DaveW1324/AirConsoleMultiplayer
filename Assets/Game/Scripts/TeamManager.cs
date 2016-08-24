using UnityEngine;
using System.Collections.Generic;

public class TeamManager
{
    protected static TeamManager _instance;

    protected List<Team> teams;

    /*
	* Find/Create/Return our one and only Quest Manager object
	* for the game
	**/
    public static TeamManager Instance
    {
        get
        {
            // If we still dont have an instance, it must not exist,
            // so lets create our own and add it to the GameManager object
            if (_instance == null)
            {
                _instance = new TeamManager();
            }

            return _instance;
        }
    }

    protected TeamManager()
    {
        teams = new List<Team>();
    }

    public List<Team> Teams {
        get
        {
            return teams;
        }
    }

    public void AddTeam(Team t)
    {
        if (!teams.Contains(t))
        {
            teams.Add(t);
        }
    }

    public bool HasCapacity()
    {
        foreach (Team t in teams)
        {
            if (t.HasCapacity)
            {
                return true;
            }
        }
        return false;
    }

    public void AssignPlayerToRandomTeam(Player player, GameObject playerPrefab)
    {
        Team selectedTeam = null;
        int lowestNumberOfPlayers = int.MaxValue;

        foreach (Team t in teams)
        {
            if (t.CurrentPlayerCount < lowestNumberOfPlayers && t.HasCapacity)
            {
                selectedTeam = t;
                lowestNumberOfPlayers = t.CurrentPlayerCount;
            }
        }

        if (selectedTeam != null)
        {
            selectedTeam.AddPlayer(player, playerPrefab);
        }
    }
}