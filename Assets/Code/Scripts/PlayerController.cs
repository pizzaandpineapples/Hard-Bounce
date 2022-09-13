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
    // Thrusters
    [SerializeField] private float thrusterPower;
    [SerializeField] private float thrusterSpeed;
    private float currentThrusterPower;
    private float currentThrusterSpeed;
    private bool isThrusterReleased;
    // Dash
    private float dashLimit = 0.625f;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashDrag;
    private float currentDrag;
    private bool isthrusterOnDuringDash = false;
    // Drift
    [SerializeField] private float driftPower;
    [SerializeField] private float driftTime;
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
        currentThrusterPower = thrusterPower;
        currentThrusterSpeed = thrusterSpeed;
        currentDrag = PlayerRigidbody2D.drag;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isthrusterOnDuringDash = false;

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
            RotateShip(inputVectorRotate.normalized);
        }
        // Only moves when thrusters are activated.
        if (playerControls.Player.Thrusters.IsPressed())
        {
            isThrusterReleased = false;
            isthrusterOnDuringDash = true;

            // PlayeRigidbody2D.AddForce(currentInputVectorMovement * thrusterPower * thrusterSpeed * Time.deltaTime);
            PlayerRigidbody2D.AddForce(transform.up.normalized * thrusterPower * thrusterSpeed * Time.deltaTime);
        }
        else
        #endregion

        #region Drift
        // R2 is released.
        // Control movement for a while without thrusters. Then you are stuck on that path until you turn on thrusters or brake.

        if (isThrusterReleased)
        {
            StartCoroutine(DriftCoroutine());
        }
        #endregion

        #region Brake
        // HOLD L2 or B key to activate brakes
        /* Let's not call it brake. This can be used for smaller more precise movements.
         * HOLD L2 and R2 for precision aim */

        // Hold [O] or V key to activate hard-brakes.
        // Stops ship immediately. Simulates car hand brake.

        if (playerControls.Player.Brake.IsPressed())
        {
            OnBrake(brakeStrengthInverse, false);
        }
        if (playerControls.Player.Hardbrake.IsPressed())
        {
            // TODO 

            OnBrake(brakeStrengthInverse, true);
            playerControls.Player.Thrusters.Disable();
        }

        playerControls.Player.Thrusters.Enable();
        
        #endregion

        #region Dash
        // Tap [X] or shift keys to dash.

        if (playerControls.Player.Dash.IsPressed())
        {
            // Basic dash
            if (!isthrusterOnDuringDash)
            {
                playerControls.Player.Dash.Disable();
                StartCoroutine(DashCoroutine(0));
            }
            else if (isthrusterOnDuringDash) // Thruster dash
            {
                playerControls.Player.Dash.Disable();
                StartCoroutine(DashCoroutine(brakeStrengthInverse));
            }
            else if (!isthrusterOnDuringDash && isThrusterReleased) // Drift dash
            {
                // DashReset(); // Drift dashing this way feels unnatural.
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

    void Update()
    {
        #region Check if thruster is released
        // R2 is released.

        if (playerControls.Player.Thrusters.WasReleasedThisFrame())
        {
            Debug.Log("Thruster is released");
            isThrusterReleased = true;
        }
        #endregion
    }

    // To rotate the player.
    void RotateShip(Vector2 direction)
    {
        // Vertical and horizontal axes were flipped. Not sure why?
        // I had to reverse the x and y for this to work somewhat correctly.
        // I had to also invert the x-axis from the InputSystem.
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // To maintain drift while thruster is released.
    IEnumerator DriftCoroutine()
    {
        Debug.Log("Enters coroutine");

        PlayerRigidbody2D.AddForce(transform.up.normalized * driftPower * Time.deltaTime);
        yield return new WaitForSeconds(driftTime);

        DashReset();
    }

    // To brake the ship.
    void OnBrake(float smoothTime, bool brakeType)
    {
        thrusterPower = 0;
        thrusterSpeed = 0;

        // Damping to zero. No hard brakes.
        PlayerRigidbody2D.velocity = Vector2.SmoothDamp(PlayerRigidbody2D.velocity, Vector2.zero, ref smoothVelocity, (smoothTime / 1000));
        PlayerRigidbody2D.angularVelocity = 0;

        thrusterPower = currentThrusterPower;
        thrusterSpeed = currentThrusterSpeed;

        if (brakeType)
        {
            DashReset();
        }
    }

    // To perform a dash.
    IEnumerator DashCoroutine(float smoothTime)
    {
        PlayerRigidbody2D.drag = dashDrag;
        PlayerRigidbody2D.AddForce(transform.up.normalized * (dashPower * 10) * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashLimit);
        OnBrake(smoothTime, true);
        PlayerRigidbody2D.drag = currentDrag;
        playerControls.Player.Dash.Enable();
    }

    // To reset some variables related to Dash.
    void DashReset()
    {
        // Need to stop all coroutines, for drifting to not glitch out.
        // Definitely not the right approach. Works for now.
        // This method is called whenever interfacing with the drift mechanic.

        // All coroutines stop mid way, so this means, some values must be reset.
        // Resets these values (Values related to dash) so that the player doesn't become slow and dash can be used again.

        StopAllCoroutines();

        PlayerRigidbody2D.drag = currentDrag;
        playerControls.Player.Dash.Enable();

        isThrusterReleased = false;
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
