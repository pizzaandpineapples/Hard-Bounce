using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Tutorial4Event : MonoBehaviour
{
    [SerializeField] private GameObject SpawnManager;

    [SerializeField] private bool diedOnce = false;

    [SerializeField] private GameObject redInstruction;
    [SerializeField] private GameObject dashInstructions;
    [SerializeField] private GameObject psImage;
    [SerializeField] private GameObject xboxImage;
    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    // Update is called once per frame
    void Update()
    {
        if (diedOnce)
        {
            dashInstructions.gameObject.SetActive(true);

            if (Gamepad.current == DualShockGamepad.current)
            {
                xboxImage.gameObject.SetActive(false);
            }
            else
            {
                psImage.gameObject.SetActive(false);
            }

            goThisWayInstruction.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        StartCoroutine(RespawnCoroutine());
        redInstruction.gameObject.SetActive(true);
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        diedOnce = true;
        yield return new WaitForSeconds(0.5f);
        SpawnManager.GetComponent<SpawnManager>().SpawnPlayer();
    }
}
