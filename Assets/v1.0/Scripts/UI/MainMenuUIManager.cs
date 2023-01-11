using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;

public class MainMenuUIManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string m_firstLevelName;
    private string m_continueLevelName;

    [SerializeField] private GameObject m_mainMenuPanel;
    [SerializeField] private GameObject m_newGameButton;
    [SerializeField] private GameObject m_continueButton;
    [SerializeField] private GameObject m_levelSelectButton;
    [SerializeField] private GameObject m_levelSelectPanel;
    [SerializeField] private GameObject m_optionsButton;
    [SerializeField] private GameObject m_quitButton;

    private bool m_isNewGameStarted = false;

    private PlayerControls m_playerControls;
    [SerializeField] private GameObject m_gameManager;
    private GameManager m_gameManagerScript;

    private void Awake()
    {
        m_playerControls = new PlayerControls();
        m_playerControls.Enable();

        m_gameManagerScript = m_gameManager.GetComponent<GameManager>();
    }

    private void Start()
    {
        m_continueButton.gameObject.SetActive(m_isNewGameStarted);
        m_levelSelectButton.gameObject.SetActive(m_isNewGameStarted);
    }

    private void Update()
    {
        if (m_levelSelectPanel.transform.localPosition == Vector3.zero)
        {
            if (m_playerControls.UI.PauseMenu.triggered || m_playerControls.UI.Cancel.triggered)
            {
                m_mainMenuPanel.SetActive(true);
                m_gameManagerScript.PauseMenuEnable();
                m_gameManagerScript.PauseMenuDisable();
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(m_levelSelectButton);
                m_levelSelectPanel.transform.localPosition = new Vector3(1333, 0, 0);
            }
        }
    }

    public void LoadData(GameData data)
    {
        if (data.levelsUnlocked.Count > 0)
        {
            m_isNewGameStarted = true;
            m_continueLevelName = data.levelsUnlocked.Last();
        }
        else
            m_isNewGameStarted = false;
    }

    public void SaveData(ref GameData data)
    {
        // Not implemented
    }

    public void OnNewGameClick()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene(m_firstLevelName, LoadSceneMode.Single);
    }

    public void OnContinueClick()
    {
        SceneManager.LoadScene(m_continueLevelName, LoadSceneMode.Single);
    }

    public void OnLevelSelectClick()
    {
        m_levelSelectPanel.transform.localPosition = Vector3.zero;
        m_mainMenuPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_levelSelectPanel.GetComponent<LevelSelector>().LevelSelectFirstButton);
    }

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
