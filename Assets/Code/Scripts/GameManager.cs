using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// using UnityEngine.Events;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string currentSceneName;

    // Pause menu
    private bool isPaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject backButton;
    private GameObject currentPointerEnter;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    private float sfxVolumeValue;
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
    private Rigidbody2D playerRigidbody2D;
    private BounceOffObjects playerBounceOffObjects;
    private PlayerController playerController;
    private AudioSource playerAudioSource;
    public float playerVelocity;
    public int bounceCount;
    public int dashCount;
    public int deathCount; 

    // Restart/Respawn
    [NonSerialized] public bool isPlayerDead = false;
    [SerializeField] private float restartTimeAfterPlayerDeath = 2f;
    [SerializeField] private float quickRestartTime = 0f;
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

    public void LoadData(GameData data)
    {
        // Audio Settings
        this.gameManagerAudioSource.volume = data.musicVolume;
        this.musicVolumeSlider.value = data.musicVolumeSliderValue;
        this.sfxVolumeValue = data.sfxVolume;
        this.sfxVolumeSlider.value = data.sfxVolumeSliderValue;

        // Level unlocks
        //data.levelsUnlocked.TryGetValue(currentSceneName, out isLevelUnlocked);

        // Player stats
        this.deathCount = data.deathCount;
    }

    public void SaveData(ref GameData data)
    {
        // Audio Settings
        data.musicVolume = this.gameManagerAudioSource.volume;
        data.musicVolumeSliderValue = this.musicVolumeSlider.value;
        if (playerAudioSource != null) data.sfxVolume = playerAudioSource.volume;
        data.sfxVolumeSliderValue = this.sfxVolumeSlider.value;

        // Level unlocks
        if (data.levelsUnlocked.ContainsKey(currentSceneName))
        {
            Debug.Log("Level already unlocked");
        }
        else
        {
            data.levelsUnlocked.Add(currentSceneName, true);
            Debug.Log("Level is unlocked");
        }

        // Player stats
        data.deathCount = this.deathCount;
    }

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        gameManagerAudioSource.PlayOneShot(levelCompleteAudioClip, levelCompleteVolume);
        SpawnPlayer();
    }

    void Update()
    {
        if (playerThatIsCurrentlySpawned != null)
        {
            if (playerBounceOffObjects != null)
            {
                isPlayerDead = playerBounceOffObjects.isPlayerDead;
                bounceCount = playerBounceOffObjects.bounceCount;
            }
            if (playerRigidbody2D != null)
                playerVelocity = playerRigidbody2D.velocity.magnitude;
            if (playerController != null)
                dashCount = playerController.dashCount;
        }

        if (isPlayerDead)
        {
            deathCount++;

            // If player is NOT respawnable, then restart the level. Else, respawn player.
            StartCoroutine(!isPlayerRespawnable ? RestartGameCoroutine(restartTimeAfterPlayerDeath) : RespawnCoroutine(restartTimeForRespawn));
            isPlayerDead = false;
            playerBounceOffObjects.isPlayerDead = false;
        }

        // BACK button/ESCAPE key to access pause menu.
        // Exit using BACK Button/ESCAPE key and also the cancel input (B/Circle on controller or ESCAPE key on keyboard)
        if (playerControls.UI.PauseMenu.triggered || playerControls.UI.Cancel.triggered)
        {
            if (isPaused)
                PauseMenuDisable();
            else
                PauseMenuEnable();
        }

        // TODO: Restart/Respawn to checkpoints.
        if (playerControls.Player.QuickRestart.IsPressed())
        {
            StartCoroutine(RestartGameCoroutine(quickRestartTime));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the player gameobject after it has been spawned.
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Spawned player");
            playerThatIsCurrentlySpawned = collision.gameObject;
            playerRigidbody2D = playerThatIsCurrentlySpawned?.GetComponent<Rigidbody2D>();
            playerBounceOffObjects = playerThatIsCurrentlySpawned?.GetComponent<BounceOffObjects>();
            playerController = playerThatIsCurrentlySpawned?.GetComponent<PlayerController>();
            playerAudioSource = playerThatIsCurrentlySpawned?.GetComponent<AudioSource>();

            playerAudioSource.volume = sfxVolumeValue;

            //sfxVolumeSlider.onValueChanged.AddListener(delegate { sfxVolumeSliderChange(sfxVolumeSlider.value); }); // Can use delegates/events too.
            sfxVolumeSlider.onValueChanged.AddListener(value => playerController.AdjustVolume(sfxVolumeSlider.value));
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
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator RestartGameCoroutine(float restartTime)
    {
        yield return new WaitForSeconds(restartTime);
        RestartGame();
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
        EventSystem.current.SetSelectedGameObject(backButton);
    }
    public void PauseMenuDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);

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
        // Debug.Log("Selected");
        if (eventData.selectedObject.GetComponent<Slider>())
            eventData.selectedObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        else
            eventData.selectedObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        // Debug.Log("Deselected");
        if (eventData.selectedObject.GetComponent<Slider>())
            eventData.selectedObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(102f / 255f, 117f / 255f, 119f / 255f, 1f);
        else
            eventData.selectedObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
    public void OnPointerEnter(BaseEventData eventData)
    {
        // Debug.Log("pointer enter");
        PointerEventData pointerData = eventData as PointerEventData;

        EventSystem.current.SetSelectedGameObject(null);

        pointerData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }
    public void OnPointerExit(BaseEventData eventData)
    {
        // Debug.Log("pointer exit");
        PointerEventData pointerData = eventData as PointerEventData;
        
        pointerData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        currentPointerEnter = pointerData.pointerEnter.transform.parent.gameObject;
        
        EventSystem.current?.SetSelectedGameObject(null);
        EventSystem.current?.SetSelectedGameObject(currentPointerEnter);
    }
    public void OnPointerEnterSlider(BaseEventData eventData)
    {
        // Debug.Log("pointer enter slider");
        PointerEventData pointerData = eventData as PointerEventData;

        EventSystem.current.SetSelectedGameObject(null);

        pointerData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
    public void OnPointerExitSlider(BaseEventData eventData)
    {
        // Debug.Log("pointer exit slider");
        PointerEventData pointerData = eventData as PointerEventData;

        pointerData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().color = new Color(102f / 255f, 117f / 255f, 119f / 255f, 1f);

        currentPointerEnter = pointerData.pointerEnter.transform.parent.gameObject;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(currentPointerEnter);
    }

    public void QuitGame()
    {
        DataPersistenceManager.instance.SaveGame();

        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
