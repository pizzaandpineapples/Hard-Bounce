using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string SceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
}
