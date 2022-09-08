using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Spaceship Properties

    [SerializeField] private float thrusterPower;
    [SerializeField] private float thrusterSpeed;
    private float currentThrusterPower;
    private float currentThrusterSpeed;

    private float dashLimit = 0.625f;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashDrag;
    private float currentDrag;
    private bool isthrusterOnDuringDash = false;

    #endregion

    #region Smoothdamp

    private Vector2 smoothVelocity; // Empty velocity reference for all Smoothdamp functions.

    // Movement Smoothdamp
    private Vector2 currentInputVectorMovement;
    [SerializeField] private float movementSmooth;

    /*
    // Rotate Smoothdamp
    private Vector2 currentInputVectorRotate;
    [SerializeField] private float rotateSmooth;
    */

    // Brake Smoothdamp
    [SerializeField] private float brakeStrengthInverse; // Strength decreases as field value increases. 100 is preferred.

    // Dash Smoothdamp
    [SerializeField] private float smoothDash;
    private float smoothVelocityDash; // Empty velocity reference for dash Smoothdamp functions.

    #endregion

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
        currentDrag = PlayeRigidbody2D.drag;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isthrusterOnDuringDash = false;

        #region Movement and Rotation
        // Left stick or WASD keys to move.
        // Right stick or mouse to rotate/aim.
        // HOLD R2 or shift keys to activate thrusters.

        Vector2 inputVectorMovement = playerInputActions.Player.Movement.ReadValue<Vector2>();
        /* maxSpeed parameter is not used, because we know that the keyboard entry will be clamped to 1.
         * currentVelocity -> ref smoothInputVelocity. We don't need the current velocity, but we need to pass it. */
        currentInputVectorMovement = Vector2.SmoothDamp(currentInputVectorMovement, inputVectorMovement, ref smoothVelocity, movementSmooth);

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
            isthrusterOnDuringDash = true;
            // PlayeRigidbody2D.drag = currentDrag; // Forces drag back to normal if you are using thrusters immediately after dashing.
            PlayeRigidbody2D.AddForce(currentInputVectorMovement * thrusterPower * thrusterSpeed * Time.deltaTime);
        }
        #endregion

        #region Brake
        // HOLD L2 or B key to activate brakes

        /* Let's not call it brake. This can be used for smaller more precise movements.
         * HOLD L2 and R2 for precision aim */

        if (playerInputActions.Player.Brake.IsPressed())
        {
            OnBrake(brakeStrengthInverse);
        }
        #endregion

        #region Dash
        // Tap [X] or shift keys to dash.

        if (playerInputActions.Player.Dash.IsPressed())
        {
            // Basic dash
            if (!isthrusterOnDuringDash)
            {
                playerInputActions.Player.Dash.Disable();
                StartCoroutine(DashCoroutine(0));
            }
            else // Thruster dash
            {
                playerInputActions.Player.Dash.Disable();
                StartCoroutine(DashCoroutine(brakeStrengthInverse));
            }
        }

        /* OLD DASH SYSTEM.
         * Triggered both actions when using a Modifier Composite.
         * Triggered both actions even if I used separated binding combos.
         *
        // Basic dash
        if (playerInputActions.Player.Dash.IsPressed())
        {
            playerInputActions.Player.Dash.Disable();
            StartCoroutine(BasicDashCoroutine(0));
        }
        // Thruster dash
        else if (playerInputActions.Player.Dash.IsPressed() && playerInputActions.Player.Thrusters.IsPressed())
        {
            playerInputActions.Player.Dash.Disable();
            StartCoroutine(BasicDashCoroutine(brakeStrengthInverse));
        } */
        #endregion
    }

    // To rotate the player.
    void RotateAim(Vector2 direction)
    {
        // Vertical and horizontal axes were flipped. Not sure why?
        // I had to reverse the x and y for this to work somewhat correctly.
        // I had to also invert the x-axis from the InputSystem.
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // To brake the ship.
    public void OnBrake(float smoothTime)
    {
        thrusterPower = 0;
        thrusterSpeed = 0;

        // Damping to zero. No hard brakes.
        PlayeRigidbody2D.velocity = Vector2.SmoothDamp(PlayeRigidbody2D.velocity, Vector2.zero, ref smoothVelocity, (smoothTime / 1000));
        PlayeRigidbody2D.angularVelocity = 0;

        thrusterPower = currentThrusterPower;
        thrusterSpeed = currentThrusterSpeed;
    }

    // To perform a dash.
    IEnumerator DashCoroutine(float smoothTime)
    {
        PlayeRigidbody2D.drag = dashDrag;
        PlayeRigidbody2D.AddForce(currentInputVectorMovement * (dashPower * 10) * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashLimit);
        OnBrake(smoothTime);
        PlayeRigidbody2D.drag = currentDrag;
        playerInputActions.Player.Dash.Enable();
    }

    /* Old thruster dash coroutine. This is accomplished by the Dash Coroutine.
     *
    // To perform thruster dash.
    IEnumerator ThrusterDashCoroutine()
    {
        PlayeRigidbody2D.drag = dashDrag;
        PlayeRigidbody2D.AddForce(currentInputVectorMovement * (dashPower / 50000) * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashLimit);
        PlayeRigidbody2D.drag = Mathf.SmoothDamp(dashDrag, currentDrag, ref smoothVelocityDash, smoothDash);
        playerInputActions.Player.Dash.Enable();
    }
    */

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
