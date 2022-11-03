using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Fond more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        // TODO: Load an saved data from a file using the data handler
        // If no data can be loaded, initialize to a new game.
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        // TODO: Push the loaded data to all other scripts that need it.
    }

    public void LoadGame()
    {
        // TODO: Pass the data to other scripts so they can update it.

        // TODO: Save that data to a file using the data handler.

    }
}
