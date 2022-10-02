using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5Manager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    [SerializeField] private int howManyDashesToNextLevel;

    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject goThisWay;
    [SerializeField] private GameObject nextLevel;

    void Update()
    {
        if (gameManager.GetComponent<GameManager>().playerVelocity > 100)
        {
            instructions.gameObject.SetActive(true);
        }

        if (gameManager.GetComponent<GameManager>().dashCount >= howManyDashesToNextLevel)
        {
            goThisWay.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }
}
