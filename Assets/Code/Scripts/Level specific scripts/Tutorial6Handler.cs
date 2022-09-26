using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial6Handler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        StartCoroutine(RestartCoroutine());
    }

    IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}