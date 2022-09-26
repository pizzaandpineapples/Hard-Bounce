using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button TutorialButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button ResetButton;

    [SerializeField] public bool isTutorialComplete;

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
            TutorialButton.gameObject.SetActive(true);
            QuitButton.gameObject.transform.localPosition = new Vector3(0, -72, 0);
            ResetButton.gameObject.transform.localPosition = new Vector3(0, -108, 0);
        }
        else
        {
            TutorialButton.gameObject.SetActive(false);
            QuitButton.gameObject.transform.localPosition = new Vector3(0, -36, 0);
            ResetButton.gameObject.transform.localPosition = new Vector3(0, -72, 0);
        }
    }

    public void PlayGame()
    {
        if (isTutorialComplete)
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Tutorial 1", LoadSceneMode.Single);
        }
    }

    public void Tutorial()
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
