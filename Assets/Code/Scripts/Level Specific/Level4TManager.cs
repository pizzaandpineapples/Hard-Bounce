using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4TManager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private GameManager gameManagerScript;

    [SerializeField] private int bouncesLevelMin;
    [SerializeField] private int bouncesLevelMax;
    [SerializeField] private int howManyBouncesToNextLevel;

    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    void Awake()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    void Start()
    {
        howManyBouncesToNextLevel = Random.Range(bouncesLevelMin, bouncesLevelMax);
    }

    void Update()
    {
        if (gameManagerScript.bounceCount >= howManyBouncesToNextLevel)
        {
            goThisWayInstruction.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }
}
