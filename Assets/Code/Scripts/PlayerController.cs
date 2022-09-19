using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    #region Spaceship
    // Input Vectors
    private Vector2 inputVectorMovement;
    private Vector2 inputVectorRotate;
    // Movement
    private bool isMovementActive;
    [SerializeField] private float movementStrength;
    private float currentMovementStrength;
    // Dash
    [SerializeField] private float dashLimit = 0.625f;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashDrag;
    private float currentDrag;
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
    #endregion

    private Rigidbody2D PlayerRigidbody2D;
    private PlayerInput PlayerInput;
    private PlayerControls playerControls;

    void Awake()
    {
        PlayerRigidbody2D = GetComponent<Rigidbody2D>();
        PlayerInput = GetComponent<PlayerInput>();

        // Enables the PlayerInputActions input action asset.
        // Subscribes a method to the Movement action in the Player action map.
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        // playerInputActions.Player.Movement.performed += MovementOnPerformed; // No longer need because we aren't calling this method anymore. It has been moved to the FixedUpdate() method.
        // playerInputActions.Player.Thrusters.canceled += ThrustersOnCanceled; // Another way of implementing Input.GetKeyUp.
    }

    void Start()
    {
        isMovementActive = true;
        currentMovementStrength = movementStrength;
        currentDrag = PlayerRigidbody2D.drag;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region Movement & Rotation of ship
        // Left stick or WASD keys to move.
        // HOLD R2 or shift keys to activate thrusters.

        inputVectorMovement = playerControls.Player.Movement.ReadValue<Vector2>();
        /* maxSpeed parameter is not used, because we know that the keyboard entry will be clamped to 1.
         * currentVelocity -> ref smoothInputVelocity. We don't need the current velocity, but we need to pass it. */
        currentInputVectorMovement = Vector2.SmoothDamp(currentInputVectorMovement, inputVectorMovement, ref smoothVelocity, movementSmooth);

        inputVectorRotate = playerControls.Player.Movement.ReadValue<Vector2>();
        // currentInputVectorRotate = Vector2.SmoothDamp(currentInputVectorRotate, inputVectorRotate, ref smoothInputVelocity, rotateSmooth); 
        /* For some reason I can't pass the Rotate Smoothdamp into the Rotate aim method.
         * All the inputs get combined. Left and right stick, both move and rotate the player. */

        // Will stay in last rotated position.
        if (inputVectorRotate != Vector2.zero)
        {
            RotatePlayer(inputVectorRotate.normalized);
        }

        if ((PlayerRigidbody2D.velocity.magnitude > 4.0f))
        {
            playerControls.Player.Movement.Enable();
        }
        else
        {
            playerControls.Player.Movement.Disable();
        }

        // Only moves when thrusters are activated.
        if (isMovementActive)
        {
            // PlayeRigidbody2D.AddForce(currentInputVectorMovement * thrusterPower * thrusterSpeed * Time.deltaTime);
            PlayerRigidbody2D.AddForce(transform.up.normalized * movementStrength * Time.deltaTime, ForceMode2D.Force);
            Debug.Log(PlayerRigidbody2D.velocity.magnitude);
        }
        else
        #endregion

        #region Brake [Only for testing]
        // Hold [O] or V key to activate hard-brakes.
        // Stops ship immediately. Simulates car hand brake.

        if (!isMovementActive)
        {
            OnBrake(brakeStrengthInverse, true);
        }
        #endregion

        #region Dash
        // Tap [X] or shift keys to dash.

        if (playerControls.Player.Dash.IsPressed())
        {
            playerControls.Player.Dash.Disable();
            StartCoroutine(DashCoroutine(brakeStrengthInverse));
        }
        #endregion
    }

    void Update()
    {
        if (playerControls.Player.Hardbrake.IsPressed())
        {
            Debug.Log("Circle is pressed");
            isMovementActive = false;
        }
        if (playerControls.Player.Hardbrake.WasReleasedThisFrame())
        {
            Debug.Log("Circle was released");
            isMovementActive = true;
        }
    }
    
    // To rotate the player.
    void RotatePlayer(Vector2 direction)
    {
        // Vertical and horizontal axes were flipped. Not sure why?
        // I had to reverse the x and y for this to work somewhat correctly.
        // I had to also invert the x-axis from the InputSystem.
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    
    // To brake the ship.
    void OnBrake(float smoothTime, bool isHardBrake)
    {
        if (!isHardBrake)
        {
            movementStrength = 0;

            // Damping to zero.
            PlayerRigidbody2D.velocity = Vector2.SmoothDamp(PlayerRigidbody2D.velocity, Vector2.zero, ref smoothVelocity, (smoothTime / 1000));
            PlayerRigidbody2D.angularVelocity = 0;

            movementStrength = currentMovementStrength;
        }
        
        if (isHardBrake)
        {
            // Hard brake.
            PlayerRigidbody2D.velocity = Vector2.zero;
            PlayerRigidbody2D.angularVelocity = 0;
        }
    }

    // To perform a dash.
    IEnumerator DashCoroutine(float smoothTime)
    {
        PlayerRigidbody2D.drag = dashDrag;
        PlayerRigidbody2D.AddForce(transform.up.normalized * (dashPower * 10) * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashLimit);
        OnBrake(smoothTime, false);
        PlayerRigidbody2D.drag = currentDrag;
        playerControls.Player.Dash.Enable();
    }





    /* Input.GetKeyUp implementation of releasing thrusters.
     * Checks when the thruster button is released.
    private void ThrustersOnCanceled(InputAction.CallbackContext obj)
    {
        Debug.Log("Thruster was released.");
        isThrusterReleased = true;
    }
    */
    /* Old thruster dash coroutine. This is accomplished by the Dash Coroutine.
     *
    // To perform thruster dash.
    IEnumerator ThrusterDashCoroutine()
    {
        PlayeRigidbody2D.drag = dashDrag;
        PlayeRigidbody2D.AddForce(currentInputVectorMovement * (dashPower / 50000) * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashLimit);
        PlayeRigidbody2D.drag = Mathf.SmoothDamp(dashDrag, currentDrag, ref smoothVelocityDash, smoothDash);
        playerControls.Player.Dash.Enable();
    }
    */
    /* This method is moved into the FixedUpdate() method.
     * This allows continuous movement with a keypress.
    private void MovementOnPerformed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        Vector2 inputVector = playerControls.Player.Movement.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputVector, ref smoothInputVelocity, inputSpeed); // maxSpeed parameter is not used, because we know that the keyboard entry will be clamped to 1.

        PlayeRigidbody2D.AddForce(currentInputVector * thrusterPower * speed * Time.deltaTime);
    }
    */
}
