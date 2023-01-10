using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private GameManager gameManagerScript;

    [SerializeField] private int keyAmountCollected;
    [SerializeField] private int keyAmountRequired;

    [SerializeField] private string SceneName;

    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            keyAmountCollected = collision.gameObject.GetComponent<PlayerController>().keyAmountCollected;
        }
        if (keyAmountCollected >= keyAmountRequired)
        {
            gameManagerScript.isLevelComplete = true;
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }
}
