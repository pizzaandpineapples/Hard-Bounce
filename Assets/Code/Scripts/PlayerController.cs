using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Spaceship properties
    [SerializeField] private float thrusterPower;
    [SerializeField] private float currentThrusterPower;
    [SerializeField] private float thrusterSpeed;
    [SerializeField] private float currentThrusterSpeed;
    //[SerializeField] private float regularSpeed;

    // Movement Smoothdamp
    private Vector2 currentInputVectorMovement;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float movementSmooth;

    /*
    // Rotate Smoothdamp
    private Vector2 currentInputVectorRotate;
    [SerializeField] private float rotateSmooth;
    */

    // Brake Smoothdamp
    [SerializeField] private float brakeStrengthInverse; // Strength decreases as field value increases. 100 is preferred.


    private Rigidbody2D PlayeRigidbody2D;
    private PlayerInput PlayerInput;
    private PlayerInputActions playerInputActions;

    // Start is called before the first frame update
    void Awake()
    {
        PlayeRigidbody2D = GetComponent<Rigidbody2D>();
        PlayerInput = GetComponent<PlayerInput>();

        // Enables the PlayerInputActions input action asset.
        // Subscribes a method to the Movement action in the Player action map.
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        // playerInputActions.Player.Movement.performed += MovementOnPerformed; // No longer need because we aren't calling this method anymore. It has been moved to the FixedUpdate() method.

        currentThrusterPower = thrusterPower;
        currentThrusterSpeed = thrusterSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thrusterPower = currentThrusterPower;
        thrusterSpeed = currentThrusterSpeed;

        #region Movement and Rotation
        // Left stick or WASD to move.
        // Right stick or mouse to rotate/aim.
        // R2 to activate thrusters.

        Vector2 inputVectorMovement = playerInputActions.Player.Movement.ReadValue<Vector2>();
        /* maxSpeed parameter is not used, because we know that the keyboard entry will be clamped to 1.
         * currentVelocity -> ref smoothInputVelocity. We don't need the current velocity, but we need to pass it. */
        currentInputVectorMovement = Vector2.SmoothDamp(currentInputVectorMovement, inputVectorMovement, ref smoothInputVelocity, movementSmooth);

        Vector2 inputVectorRotate = playerInputActions.Player.Look.ReadValue<Vector2>();
        // currentInputVectorRotate = Vector2.SmoothDamp(currentInputVectorRotate, inputVectorRotate, ref smoothInputVelocity, rotateSmooth); 
        /* For some reason the I can't pass the Rotate Smoothdamp into the Rotate aim method.
         * All the inputs get combined. Left and right stick, both move and rotate the player. */

        // Will stay in last rotated position.
        if (inputVectorRotate != Vector2.zero)
        {
            RotateAim(inputVectorRotate.normalized);
        }
        //Only moves when thrusters are activated.
        if (playerInputActions.Player.Thrusters.IsPressed())
        {
            PlayeRigidbody2D.AddForce(currentInputVectorMovement * thrusterPower * thrusterSpeed * Time.deltaTime);
        }
        #endregion

        #region Brake
        // L2 to activate brakes

        /* Let's not call it brake. This can be used for smaller more precise movements. */

        if (playerInputActions.Player.Brake.IsPressed())
        {
            thrusterPower = 0;
            thrusterSpeed = 0;

            // PlayeRigidbody2D.velocity = Vector2.zero;
            // Damping to zero. No hard brakes.
            PlayeRigidbody2D.velocity = Vector2.SmoothDamp(PlayeRigidbody2D.velocity, Vector2.zero, ref smoothInputVelocity, (brakeStrengthInverse / 1000));
            PlayeRigidbody2D.angularVelocity = 0;
        }
        #endregion

        #region Dash/Dodge



        #endregion
    }

    // To rotate the player.
    public void RotateAim(Vector2 direction)
    {
        // Vertical and horizontal axes were flipped. Not sure why?
        // I had to reverse the x and y for this to work somewhat correctly.
        // I had to also invert the x-axis from the InputSystem.
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    /* This method is moved into the FixedUpdate() method.
     * This allows continuous movement with a keypress.
    private void MovementOnPerformed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputVector, ref smoothInputVelocity, inputSpeed); // maxSpeed parameter is not used, because we know that the keyboard entry will be clamped to 1.

        PlayeRigidbody2D.AddForce(currentInputVector * thrusterPower * speed * Time.deltaTime);
    }
    */
}
