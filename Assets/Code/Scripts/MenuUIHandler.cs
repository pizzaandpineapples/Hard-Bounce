using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;


public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private GameObject currentPointerEnter;
    
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
        //Debug.Log("Selected");
        eventData.selectedObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        //Debug.Log("Deselected");
        eventData.selectedObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
    public void OnPointerEnter(BaseEventData eventData)
    {
        Debug.Log("pointer enter");
        PointerEventData pointerData =  eventData as PointerEventData;
        currentPointerEnter = pointerData.pointerEnter.gameObject;
        pointerData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(currentPointerEnter);
    }
    public void OnPointerExit(BaseEventData eventData)
    {
        Debug.Log("pointer exit");
        PointerEventData pointerData = eventData as PointerEventData;
        pointerData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(currentPointerEnter);
    }

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
