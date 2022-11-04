using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;

    public SerializableDictionary<string, bool> levelsUnlocked;

    // The values defined in this constructor will be the default values
    // the game starts with when there is no data to load or we start a new game.
    public GameData()
    {
        this.deathCount = 0;

        levelsUnlocked = new SerializableDictionary<string, bool>();
    }
}
