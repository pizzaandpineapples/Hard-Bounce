using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Vector2 inputVectorAim;
    private Vector2 currentInputVectorAim;

    private Vector2 smoothInputVelocity;
    [SerializeField] private float aimSmooth;

    private PlayerControls playerControls;

    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        }

    void FixedUpdate()
    {
        inputVectorAim = playerControls.Player.Look.ReadValue<Vector2>();
        // All the inputs get combined. Left and right stick, both move and rotate the player.

        currentInputVectorAim = Vector2.SmoothDamp(currentInputVectorAim, inputVectorAim, ref smoothInputVelocity, aimSmooth); 

        // Will stay in last rotated position.
        if (currentInputVectorAim != Vector2.zero)
        {
            RotateAim(currentInputVectorAim.normalized);
        }
    }

    void RotateAim(Vector2 direction)
    {
        // Vertical and horizontal axes were flipped. Not sure why?
        // I had to reverse the x and y for this to work somewhat correctly.
        // I had to also invert the x-axis from the InputSystem.
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
