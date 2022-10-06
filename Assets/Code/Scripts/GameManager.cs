using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected GameObject playerPrefab;
    [SerializeField] protected Transform playerSpawnPosition;
    [SerializeField] protected GameObject playerThatIsCurrentlySpawned;
    [SerializeField] protected Collider2D collider;

    // TODO: Make GameManager persistent or save these values, so that they can be utilized for scoring/challenge/achievement systems.
    public float playerVelocity;
    public int bounceCount;
    public int dashCount;
    public int playerDeathCount; 

    [NonSerialized] public bool isPlayerDead = false;
    [SerializeField] private float restartTimeAfterPlayerDeath = 2f;
    [SerializeField] private float restartTimeForMenu = 0f;
    public bool isPlayerRespawnable;
    [SerializeField] private float restartTimeForRespawn = 2f;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Awake()
    {
        collider.gameObject.SetActive(true);
        playerThatIsCurrentlySpawned = playerPrefab;
    }

    void Start()
    {
        SpawnPlayer();
    }

    void Update()
    {
        isPlayerDead = playerThatIsCurrentlySpawned.GetComponent<BounceOffObjects>().isPlayerDead;
        playerVelocity = playerThatIsCurrentlySpawned.GetComponent<Rigidbody2D>().velocity.magnitude; 
        bounceCount = playerThatIsCurrentlySpawned.GetComponent<BounceOffObjects>().bounceCount;
        dashCount = playerThatIsCurrentlySpawned.GetComponent<PlayerController>().dashCount;

        if (isPlayerDead)
        {
            playerDeathCount++;

            // If player is NOT respawnable, then restart the level. Else, respawn player.
            StartCoroutine(!isPlayerRespawnable ? RestartGame(restartTimeAfterPlayerDeath) : RespawnCoroutine(restartTimeForRespawn));
            isPlayerDead = false;
            playerThatIsCurrentlySpawned.GetComponent<BounceOffObjects>().isPlayerDead = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RestartGame(restartTimeForMenu));
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the player gameobject after it has been spawned.
        if (collision.gameObject.tag == "Player")
        {
            playerThatIsCurrentlySpawned = collision.gameObject;
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

    IEnumerator RestartGame(float restartTime)
    {
        yield return new WaitForSeconds(restartTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
    }
}
