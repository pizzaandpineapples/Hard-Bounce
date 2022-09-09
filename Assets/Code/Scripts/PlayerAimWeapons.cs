using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimWeapons : MonoBehaviour
{
    private Vector2 inputVectorAim;
    private Vector2 currentInputVectorAim;

    private Vector2 smoothInputVelocity;
    [SerializeField] private float aimSmooth;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPosition; // A separate transform variable is used so that we can manually change the food spawn location as per our need.

    [SerializeField] private float weaponSpeed;
    private float weaponSpeedTimer;

    private PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    void Start()
    {
        weaponSpeedTimer = Time.time;
    }

    void Update()
    {
        #region Aim
        // Right stick to aim.

        // All the inputs get combined. Left and right stick, both move and rotate the player.
        inputVectorAim = playerControls.Player.Look.ReadValue<Vector2>();
        currentInputVectorAim = Vector2.SmoothDamp(currentInputVectorAim, inputVectorAim, ref smoothInputVelocity, aimSmooth);

        // Will stay in last rotated position.
        if (currentInputVectorAim != Vector2.zero)
        {
            RotateAim(currentInputVectorAim.normalized);
        }
        #endregion

        #region Fire Weapon
        // Right stick to fire in direction of aim.

        weaponSpeedTimer += Time.deltaTime;
        // Debug.Log(weaponTimer);
        if (weaponSpeedTimer >= weaponSpeed)
        {
            // Fire when the Right stick is moved.
            if (playerControls.Player.Look.inProgress)
            {
                // Uses the transforms rotation to make the ammo rotate in the direction the player is aiming.
                Instantiate(projectilePrefab, projectileSpawnPosition.position, transform.rotation); 
                // Debug.Log(weaponTimer);
                weaponSpeedTimer = 0;
            }
        }
        #endregion

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
