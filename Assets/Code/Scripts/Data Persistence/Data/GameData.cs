using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Difficulty Settings

    // Audio Settings
    public float musicVolume;

    // Level unlocks
    public SerializableDictionary<string, bool> levelsUnlocked;

    // Player stats
    public int deathCount;


    // The values defined in this constructor will be the default values
    // the game starts with when there is no data to load or we start a new game.
    public GameData()
    {
        this.musicVolume = 0.3f;

        levelsUnlocked = new SerializableDictionary<string, bool>();

        this.deathCount = 0;
    }
}
