using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Level3TManager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private GameManager gameManagerScript;

    [SerializeField] private int howManyPlayerDeathsToNextLevel;
    [SerializeField] private bool hasPlayerDiedOnce;

    [SerializeField] private GameObject redInstruction;
    [SerializeField] private GameObject dashInstructions;
    [SerializeField] private GameObject playStationControllerImage;
    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    void Awake()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.playerDeathCount >= howManyPlayerDeathsToNextLevel)
        {
            StartCoroutine(ShowInstructionsCoroutine());
        }

        if (hasPlayerDiedOnce)
        {
            dashInstructions.gameObject.SetActive(true);

            //if (Gamepad.current == DualShockGamepad.current)
            //{
            //    playStationControllerImage.gameObject.SetActive(true);
            //}
            //else
            //{
            //    playStationControllerImage.gameObject.SetActive(false);
            //}

            goThisWayInstruction.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }

    IEnumerator ShowInstructionsCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        redInstruction.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        hasPlayerDiedOnce = true;
    }
}
