using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPosition;
    private GameObject playerThatIsCurrentlySpawned;
    [SerializeField] private Collider2D collider;

    [SerializeField] private int bounceCount;
    [SerializeField] private int howManyBouncesToNextLevel;
    [SerializeField] private int dashCount;
    [SerializeField] private int howManyDashes;

    void Awake()
    {
        collider.gameObject.SetActive(true);
    }

    void Start()
    {
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
    public void SpawnPlayer()
    {
        Vector3 rotation = new Vector3(0, 0, Random.Range(0, 361));
        Instantiate(playerPrefab, playerSpawnPosition.position, Quaternion.Euler(rotation));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the player gameobject after it has spawned.
        if (collision.gameObject.tag == "Player")
        {
            playerThatIsCurrentlySpawned = collision.gameObject;
        }
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
