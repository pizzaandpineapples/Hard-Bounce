using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4Handler : MonoBehaviour
{
    [SerializeField] private GameObject SpawnManager;

    [SerializeField] private bool diedOnce = false;

    [SerializeField] private GameObject red;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject goThisWay;
    [SerializeField] private GameObject nextLevel;

    // Update is called once per frame
    void Update()
    {
        if (diedOnce)
        {
            instructions.gameObject.SetActive(true);
            goThisWay.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        StartCoroutine(RespawnCoroutine());
        red.gameObject.SetActive(true);
    }

    IEnumerator RespawnCoroutine()
    {

        yield return new WaitForSeconds(1.5f);
        diedOnce = true;
        yield return new WaitForSeconds(1);
        SpawnManager.GetComponent<SpawnManager>().SpawnPlayer();
    }
}
