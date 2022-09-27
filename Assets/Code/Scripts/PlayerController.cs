using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    #region Ball
    // Input Vectors
    private Vector2 inputVectorRotate;
    // Movement
    private bool isMovementActive;
    [SerializeField] private float movementStrength;
    private float currentMovementStrength;
    [SerializeField] private float minimumVelocityToControlShip;
    // Dash
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashLimit;
    [SerializeField] private float dashStrength;
    [SerializeField] private float dashDrag;
    private float currentDrag;
    // Collision detection
    [SerializeField] private bool isColliding = false;
    [SerializeField] public int keyAmountCollected;
    #endregion

    #region Smoothdamp
    private Vector2 smoothVelocity; // Empty velocity reference for all Smoothdamp functions.
    // Movement
    //private Vector2 currentInputVectorRotate;
    //[SerializeField] private float smoothRotate;
    // Dash Smoothdamp
    [SerializeField] private float dashStrengthInverse; // Strength decreases as field value increases. 100 is preferred.
    [SerializeField] private float smoothDash;
    #endregion

    private Rigidbody2D PlayerRigidbody2D;
    // private PlayerInput PlayerInput;
    public PlayerControls playerControls;
    [SerializeField] private bool isPlayerControlEnabled;
    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip DashFX;

    void Awake()
    {
        PlayerRigidbody2D = GetComponent<Rigidbody2D>();
        // PlayerInput = GetComponent<PlayerInput>();

        // Enables the playerControls input action asset.
        playerControls = new PlayerControls();

        playerAudioSource = GetComponent<AudioSource>();
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

        inputVectorRotate = playerControls.Player.Movement.ReadValue<Vector2>();
        //currentInputVectorRotate = Vector2.SmoothDamp(currentInputVectorRotate, inputVectorRotate, ref smoothVelocity, smoothRotate);

        // Will stay in last rotated position.
        if (inputVectorRotate != Vector2.zero)
        {
            RotatePlayer(inputVectorRotate.normalized);
        }

        // Limiting player control when under a certain velocity.
        // This limiter is off when player is colliding.
        if (isColliding)
        {
            playerControls.Player.Movement.Enable();
        }
        else if (!isColliding)
        {
            if (PlayerRigidbody2D.velocity.magnitude > minimumVelocityToControlShip)
            {
                playerControls.Player.Movement.Enable();
            }
            else
            {
                playerControls.Player.Movement.Disable();
            }
        }

            // If statement only for brake.
        if (isMovementActive)
        {
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
            OnBrake();
        }
        #endregion

        #region Dash
        // Tap [X] or shift keys to dash.

        if (playerControls.Player.Dash.IsPressed())
        {
            if (PlayerRigidbody2D.velocity.magnitude < 1000)
            {
                playerControls.Player.Dash.Disable();
                StartCoroutine(DashCoroutine(dashStrengthInverse));
            }
            else if (PlayerRigidbody2D.velocity.magnitude > 100)
            {
                OnBrake();
                playerControls.Player.Dash.Disable();
                StartCoroutine(DashCoroutine(dashStrengthInverse));
            }
        }
        #endregion

        #region Control Enable/Disable
        if (isPlayerControlEnabled)
        {
            playerControls.Player.Enable();
        }
        else
        {
            playerControls.Player.Disable();
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
    void OnBrake()
    {
        PlayerRigidbody2D.velocity = Vector2.zero;
        PlayerRigidbody2D.angularVelocity = 0;
    
    }

    // To perform a dash.
    IEnumerator DashCoroutine(float smoothTime)
    {
        playerAudioSource.PlayOneShot(DashFX, 0.3f);

        // Apply a force
        PlayerRigidbody2D.drag = dashDrag;
        PlayerRigidbody2D.AddForce(transform.up.normalized * (dashStrength * 10) * Time.deltaTime, ForceMode2D.Impulse);
        
        // Duration of the force
        yield return new WaitForSeconds(dashDuration);
        
        // Make the player come to stop using smoothdamp.
        movementStrength = 0;

        PlayerRigidbody2D.velocity = Vector2.SmoothDamp(PlayerRigidbody2D.velocity, Vector2.zero, ref smoothVelocity, (smoothTime / 1000));
        PlayerRigidbody2D.angularVelocity = 0;

        movementStrength = currentMovementStrength;
        PlayerRigidbody2D.drag = currentDrag;

        // Time before you can use dash again.
        yield return new WaitForSeconds(dashLimit);

        // Enable dash again.
        playerControls.Player.Dash.Enable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            keyAmountCollected++;
            Destroy(collision.gameObject);
        }
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
