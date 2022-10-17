using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;


public class MenuUIHandler : MonoBehaviour, ISelectHandler//, IDeselectHandler
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    public TextMeshProUGUI text;
    [NonSerialized] public bool isNewGameStarted = false;

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

    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.black;
    }

    //public void OnDeselect(BaseEventData eventData)
    //{
    //    newGameButton.GetComponentInChildren<Text>().color = new Color(0, 0, 100, 100);
    //}

    public void StartNewGame()
    {
        SceneManager.LoadScene("Tutorial 1", LoadSceneMode.Single);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Current-Scene"), LoadSceneMode.Single);
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
