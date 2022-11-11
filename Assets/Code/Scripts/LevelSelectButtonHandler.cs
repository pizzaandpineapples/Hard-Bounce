using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButtonHandler : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<string> sceneNames;
    [SerializeField] private List<GameObject> levelButtons;

    public void LoadData(GameData data)
    {
        Debug.Log("Scenes loaded");
        sceneNames = data.levelsUnlocked;
    }

    public void SaveData(ref GameData data)
    {
        
    }

    void Start()
    {
        levelButtons = new List<GameObject>();
        for (int i = 1; i < sceneNames.Count; i++)
        {
            levelButtons.Add(gameObject.transform.GetChild(i - 1).gameObject);
        }

        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].SetActive(true);
        }
    }
}
