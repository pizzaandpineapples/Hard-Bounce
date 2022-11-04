using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuUIHandler : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private bool isNewGameStarted = false;

    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(ref GameData data)
    {
        
    }

    void Update()
    {
        if (PlayerPrefs.GetString("isNewGameStarted") == "true")
        {
            isNewGameStarted = true;
        }
        else if (PlayerPrefs.GetString("isNewGameStarted") == "false")
        {
            isNewGameStarted = false;
        }

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
        SceneManager.LoadScene("Level 1 T", LoadSceneMode.Single);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Current-Scene"), LoadSceneMode.Single);
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
