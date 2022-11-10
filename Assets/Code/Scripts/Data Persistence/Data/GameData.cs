using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Difficulty Settings

    // Level unlocks
    public Dictionary<string, bool> levelsUnlocked;

    // Player stats
    public int deathCount;


    // The values defined in this constructor will be the default values
    // the game starts with when there is no data to load or we start a new game.
    public GameData()
    {
        levelsUnlocked = new Dictionary<string, bool>();

        deathCount = 0;
    }
}
