using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSelector : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<string> _sceneNames;

    public GameObject ButtonPrefab;
    public GameObject ButtonParent;
    public GameObject LevelSelectFirstButton;

    public void LoadData(GameData data)
    {
        Debug.Log("Scenes loaded");
        _sceneNames = data.levelsUnlocked;
    }
    public void SaveData(ref GameData data)
    {

    }

    void Start()
    {
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        for (int i = 1; i < _sceneNames.Count; i++)
        {
            int level = i;
            GameObject newButton = Instantiate(ButtonPrefab, ButtonParent.transform);
            newButton.GetComponent<LevelSelectButton>().LevelText.text = _sceneNames[i];
            newButton.GetComponent<LevelSelectButton>().ButtonID = i.ToString();

            if (newButton.GetComponent<LevelSelectButton>().ButtonID == "1")
            {
                LevelSelectFirstButton = newButton;
            }

            newButton.GetComponent<Button>().onClick.AddListener(() => SelectLevel(level));
        }
    }

    private void SelectLevel(int level)
    {
        Debug.Log(level);
        SceneManager.LoadScene(_sceneNames[level], LoadSceneMode.Single);
    }
}
