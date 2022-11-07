using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuUIHandler : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string firstLevelName;
    [SerializeField] private string continueLevelName;

    [SerializeField] private GameObject newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private bool isNewGameStarted = false;

    public void LoadData(GameData data)
    {
        if (data.levelsUnlocked.Count > 0)
        {
            isNewGameStarted = true;
            continueLevelName = data.levelsUnlocked.Keys.Last();
        }
        else
            isNewGameStarted = false;
    }

    public void SaveData(ref GameData data)
    {
        
    }

    void Update()
    {
        if (isNewGameStarted)
        {
            newGameButton.gameObject.transform.localPosition = new Vector3(0, 72, 0);
            continueButton.gameObject.SetActive(true);
            levelSelectButton.gameObject.SetActive(true);
            optionsButton.gameObject.transform.localPosition = new Vector3(0, -36, 0);
            quitButton.gameObject.transform.localPosition = new Vector3(0, -72, 0);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
            levelSelectButton.gameObject.SetActive(false);
            optionsButton.gameObject.transform.localPosition = new Vector3(0, -0, 0);
            quitButton.gameObject.transform.localPosition = new Vector3(0, -36, 0);
        }
    }

    public void StartNewGame()
    {
        DataPersistenceManager.instance.NewGame();
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(firstLevelName, LoadSceneMode.Single);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(continueLevelName, LoadSceneMode.Single);
    }

    public void LevelSelect()
    {
        levelSelectMenu.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
