using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected GameObject playerPrefab;
    [SerializeField] protected Transform playerSpawnPosition;
    [SerializeField] protected GameObject playerThatIsCurrentlySpawned = null;
    [SerializeField] protected Collider2D collider;

    public int bounceCount;
    public int dashCount;

    [SerializeField] protected int dashesMin;
    [SerializeField] protected int dashesMax;
    public int howManyDashesToNextLevel;

    void Awake()
    {
        collider.gameObject.SetActive(true);
    }

    void Start()
    {
        howManyDashesToNextLevel = Random.Range(dashesMin, dashesMax);

        SpawnPlayer();
    }

    void Update()
    {
        bounceCount = playerThatIsCurrentlySpawned.GetComponent<BounceOffObjects>().bounceCount;
        dashCount = playerThatIsCurrentlySpawned.GetComponent<PlayerController>().dashCount;

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            BackToMainMenu();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the player gameobject after it has spawned.
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
