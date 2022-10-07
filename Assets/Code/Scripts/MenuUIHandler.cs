using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resetButton;

    [NonSerialized] public bool isTutorialComplete;

    void Start()
    {

    }
    
    void Update()
    {
        if (PlayerPrefs.GetString("isTutorialComplete") == "true")
        {
            isTutorialComplete = true;
        }
        else if (PlayerPrefs.GetString("isTutorialComplete") == "false")
        {
            isTutorialComplete = false;
        }

        if (isTutorialComplete)
        {
            tutorialButton.gameObject.SetActive(true);
            optionsButton.gameObject.transform.localPosition = new Vector3(0, -72, 0);
            quitButton.gameObject.transform.localPosition = new Vector3(0, -108, 0);
            resetButton.gameObject.transform.localPosition = new Vector3(0, -144, 0);
        }
        else
        {
            tutorialButton.gameObject.SetActive(false);
            optionsButton.gameObject.transform.localPosition = new Vector3(0, -36, 0);
            quitButton.gameObject.transform.localPosition = new Vector3(0, -72, 0);
            resetButton.gameObject.transform.localPosition = new Vector3(0, -108, 0);
        }
    }

    public void StartGame()
    {
        if (isTutorialComplete)
        {
            SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Tutorial 1", LoadSceneMode.Single);
        }
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial 1", LoadSceneMode.Single);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void Reset()
    {
        PlayerPrefs.SetString("isTutorialComplete", "false");
        PlayerPrefs.Save();
    }
}
