using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private int keyAmountCollected;
    [SerializeField] private int keyAmountRequired;

    [SerializeField] private string SceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            keyAmountCollected = collision.gameObject.GetComponent<PlayerController>().keyAmountCollected;
        }
        if (keyAmountCollected >= keyAmountRequired)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }
}
