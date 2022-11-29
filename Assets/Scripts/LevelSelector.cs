using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<string> sceneNames;

    public GameObject buttonPrefab;
    public GameObject buttonParent;

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
        for (int i = 1; i < sceneNames.Count; i++)
        {
            int level = i;
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            newButton.GetComponent<LevelSelectButton>().levelText.text = sceneNames[i];
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectLevel(level));
        }
    }

    private void SelectLevel(int level)
    {
        Debug.Log(level);
        SceneManager.LoadScene(sceneNames[level], LoadSceneMode.Single);
    }
}
