using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Tutorial4Manager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    [SerializeField] private int howManyPlayerDeathsToNextLevel;
    [SerializeField] private bool hasPlayerDiedOnce = false;

    [SerializeField] private GameObject redInstruction;
    [SerializeField] private GameObject dashInstructions;
    [SerializeField] private GameObject playStationControllerImage;
    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetComponent<GameManager>().playerDeathCount >= howManyPlayerDeathsToNextLevel)
        {
            StartCoroutine(ShowInstructionsCoroutine());
        }

        if (hasPlayerDiedOnce)
        {
            dashInstructions.gameObject.SetActive(true);

            if (Gamepad.current == DualShockGamepad.current)
            {
                playStationControllerImage.gameObject.SetActive(true);
            }
            else
            {
                playStationControllerImage.gameObject.SetActive(false);
            }

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
