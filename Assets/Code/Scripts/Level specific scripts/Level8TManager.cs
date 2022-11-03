using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem;

public class Level8TManager : MonoBehaviour
{
    [SerializeField] private static bool hasGotStuckOnce;

    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject playStationControllerImage;
    [SerializeField] private GameObject xboxControllerImage;
    [SerializeField] private GameObject nextLevel;

    public PlayerControls playerControls;

    void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
    }

    void Update()
    {
        if (hasGotStuckOnce)
        {
            instructions.gameObject.SetActive(true);

            //if (Gamepad.current == DualShockGamepad.current)
            //{
            //    playStationControllerImage.gameObject.SetActive(true);
            //    xboxControllerImage.gameObject.SetActive(false);
            //}
            //else
            //{
            //    playStationControllerImage.gameObject.SetActive(false);
            //    xboxControllerImage.gameObject.SetActive(true);
            //}
        }

        if (playerControls.Player.QuickRestart.IsPressed())
        {
            nextLevel.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        hasGotStuckOnce = true;
    }
}
