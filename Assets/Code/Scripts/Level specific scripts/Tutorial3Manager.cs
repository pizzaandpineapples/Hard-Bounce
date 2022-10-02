using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3Manager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    [SerializeField] private int bouncesLevelMin;
    [SerializeField] private int bouncesLevelMax;
    [SerializeField] private int howManyBouncesToNextLevel;

    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    void Start()
    {
        howManyBouncesToNextLevel = Random.Range(bouncesLevelMin, bouncesLevelMax);
    }

    void Update()
    {
        if (gameManager.GetComponent<GameManager>().bounceCount >= howManyBouncesToNextLevel)
        {
            goThisWayInstruction.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }
}
