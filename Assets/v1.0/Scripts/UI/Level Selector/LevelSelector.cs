using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSelector : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<string> m_sceneNames;

    [SerializeField] private GameObject m_buttonPrefab;
    [SerializeField] private GameObject m_buttonParent;
    public GameObject LevelSelectFirstButton;

    public void LoadData(GameData data)
    {
        Debug.Log("Scenes loaded");
        m_sceneNames = data.levelsUnlocked;
    }
    public void SaveData(ref GameData data)
    {
        // Not implemented
    }

    void Start()
    {
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        for (int i = 1; i < m_sceneNames.Count; i++)
        {
            int level = i;
            GameObject newButton = Instantiate(m_buttonPrefab, m_buttonParent.transform);
            newButton.GetComponent<LevelSelectButton>().LevelText.text = m_sceneNames[i];
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
        SceneManager.LoadScene(m_sceneNames[level], LoadSceneMode.Single);
    }
}
