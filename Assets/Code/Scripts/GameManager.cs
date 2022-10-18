using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// using UnityEngine.Events;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private string currentSceneName;

    // Pause menu
    private bool isPaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject backButton;
    private GameObject currentPointerEnter;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private AudioClip pauseMenuAudioClip;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float pauseMenuVolume;

    [SerializeField] private AudioClip levelCompleteAudioClip;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float levelCompleteVolume;

    // public delegate void SfxVolumeSliderChange(float volume); // Not using this delegate anymore.
    // public static SfxVolumeSliderChange sfxVolumeSliderChange; 

    private AudioSource gameManagerAudioSource;

    // Player spawn
    [SerializeField] protected GameObject playerPrefab;
    [SerializeField] protected Transform playerSpawnPosition;
    [SerializeField] protected GameObject playerThatIsCurrentlySpawned;
    [SerializeField] protected Collider2D collider;

    // TODO: Make GameManager persistent or save these values, so that they can be utilized for scoring/challenge/achievement systems.
    // Player properties
    public float playerVelocity;
    public int bounceCount;
    public int dashCount;
    public int playerDeathCount; 

    // Restart/Respawn
    [NonSerialized] public bool isPlayerDead = false;
    [SerializeField] private float restartTimeAfterPlayerDeath = 2f;
    [SerializeField] private float restartTimeForMenu = 0f;
    public bool isPlayerRespawnable;
    [SerializeField] private float restartTimeForRespawn = 2f;

    public PlayerControls playerControls;

    void Awake()
    {
        collider.gameObject.SetActive(true);
        playerThatIsCurrentlySpawned = playerPrefab;

        playerControls = new PlayerControls();
        playerControls.Enable();
        playerControls.UI.Cancel.Disable();

        gameManagerAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        gameManagerAudioSource.PlayOneShot(levelCompleteAudioClip, levelCompleteVolume);
        SpawnPlayer();
    }

    void Update()
    {
        if (playerThatIsCurrentlySpawned != null)
        {
            isPlayerDead = playerThatIsCurrentlySpawned.GetComponent<BounceOffObjects>().isPlayerDead;
            playerVelocity = playerThatIsCurrentlySpawned.GetComponent<Rigidbody2D>().velocity.magnitude;
            bounceCount = playerThatIsCurrentlySpawned.GetComponent<BounceOffObjects>().bounceCount;
            dashCount = playerThatIsCurrentlySpawned.GetComponent<PlayerController>().dashCount;
        }

        if (isPlayerDead)
        {
            playerDeathCount++;

            // If player is NOT respawnable, then restart the level. Else, respawn player.
            StartCoroutine(!isPlayerRespawnable ? RestartGameCoroutine(restartTimeAfterPlayerDeath) : RespawnCoroutine(restartTimeForRespawn));
            isPlayerDead = false;
            playerThatIsCurrentlySpawned.GetComponent<BounceOffObjects>().isPlayerDead = false;
        }

        if (playerControls.UI.PauseMenu.triggered || playerControls.UI.Cancel.triggered)
        {
            if (isPaused)
                PauseMenuDisable();
            else
                PauseMenuEnable();
        }

        if (playerControls.Player.QuickRestart.IsPressed())
        {
            StartCoroutine(RestartGameCoroutine(restartTimeForMenu));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            QuitGame();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the player gameobject after it has been spawned.
        if (collision.gameObject.tag == "Player")
        {
            playerThatIsCurrentlySpawned = collision.gameObject;
            //sfxVolumeSlider.onValueChanged.AddListener(delegate { sfxVolumeSliderChange(sfxVolumeSlider.value); }); // Can use delegates/events too.
            sfxVolumeSlider.onValueChanged.AddListener(value => collision.GetComponent<PlayerController>().AdjustVolume(sfxVolumeSlider.value));
        }
    }

    public void SpawnPlayer()
    {
        Vector3 rotation = new Vector3(0, 0, Random.Range(0, 361));
        Instantiate(playerPrefab, playerSpawnPosition.position, Quaternion.Euler(rotation));
    }
    IEnumerator RespawnCoroutine(float restartTime)
    {
        yield return new WaitForSeconds(restartTime);
        SpawnPlayer();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator RestartGameCoroutine(float restartTime)
    {
        yield return new WaitForSeconds(restartTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseMenuEnable()
    {
        isPaused = true;
        playerControls.UI.Cancel.Enable();
        pauseMenu.SetActive(true);
        Time.timeScale = 0; // Setting the Time.timeScale to 0 makes it so that physics calculations are paused.
        gameManagerAudioSource.Pause();
        gameManagerAudioSource.PlayOneShot(pauseMenuAudioClip, pauseMenuVolume);

        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        //EventSystem.current.SetSelectedGameObject(restartButton);
        EventSystem.current.SetSelectedGameObject(backButton);
    }
    public void PauseMenuDisable()
    {
        isPaused = false;
        playerControls.UI.Cancel.Disable();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        gameManagerAudioSource.PlayOneShot(pauseMenuAudioClip, pauseMenuVolume);
        gameManagerAudioSource.UnPause();

        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        if (newGameButton == null)
            EventSystem.current.SetSelectedGameObject(null);
        else
            EventSystem.current.SetSelectedGameObject(newGameButton);
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
        PointerEventData pointerData = eventData as PointerEventData;

        pointerData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

        currentPointerEnter = pointerData.pointerEnter.transform.parent.gameObject;

        //EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(currentPointerEnter);
    }
    public void OnPointerExit(BaseEventData eventData)
    {
        Debug.Log("pointer exit");
        PointerEventData pointerData = eventData as PointerEventData;
        //pointerData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    public void QuitGame()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("Current-Scene", currentSceneName);
        PlayerPrefs.Save();

        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
