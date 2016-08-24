using UnityEngine;
using System.Collections.Generic;

public class GameManager
{
    protected static GameManager _instance;

    /*
	* Find/Create/Return our one and only Quest Manager object
	* for the game
	**/
    public static GameManager Instance
    {
        get
        {
            // If we still dont have an instance, it must not exist,
            // so lets create our own and add it to the GameManager object
            if (_instance == null)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }

    protected GameManager()
    {
        
    }

    protected void CheckForWinner()
    {
        int teamsRemaining = 0;

        foreach (Team t in TeamManager.Instance.Teams)
        {
            if (t.StillAlive)
            {
                teamsRemaining++;
            }
        }

        if (teamsRemaining == 1)
        {
            Debug.LogError("Check for a winner!");
        }
    }
}