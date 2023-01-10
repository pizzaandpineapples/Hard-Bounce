using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level11Manager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private GameManager gameManagerScript;

    [SerializeField] private int howManyDashesToNextLevel;

    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    void Awake()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManagerScript.playerVelocity > 100)
        {
            instructions.gameObject.SetActive(true);
        }

        if (gameManagerScript.dashCount >= howManyDashesToNextLevel)
        {
            goThisWayInstruction.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }
}
