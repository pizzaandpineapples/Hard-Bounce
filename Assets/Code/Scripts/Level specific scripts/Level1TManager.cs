using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TManager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    [SerializeField] private int bouncesLevelMin;
    [SerializeField] private int bouncesLevelMax;
    [SerializeField] private int howManyBouncesToNextLevel;

    [SerializeField] private GameObject controllerInstruction;
    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    void Start()
    {
        howManyBouncesToNextLevel = Random.Range(bouncesLevelMin, bouncesLevelMax);

        StartCoroutine(ControllerInstructionCoroutine());

        PlayerPrefs.SetString("isNewGameStarted", "true");
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (gameManager.GetComponent<GameManager>().bounceCount >= howManyBouncesToNextLevel)
        {
            goThisWayInstruction.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }

    IEnumerator ControllerInstructionCoroutine()
    {
        yield return new WaitForSeconds(10f);
        controllerInstruction.gameObject.SetActive(false);
    }
}
