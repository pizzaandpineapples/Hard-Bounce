using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial6Manager : MonoBehaviour
{
    [SerializeField] private static bool hasGotStuckOnce;

    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    void Update()
    {
        if (hasGotStuckOnce)
        {
            instructions.gameObject.SetActive(true);
            goThisWayInstruction.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        hasGotStuckOnce = true;
    }
}
