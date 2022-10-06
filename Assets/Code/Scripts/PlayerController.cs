using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    // Input Vector
    private Vector2 inputVectorRotate;
    // Movement
    private bool isMovementActive;
    [SerializeField] private float movementStrength;
    private float currentMovementStrength;
    [SerializeField] private float minimumVelocityToControlPlayer;
    // Dash
    public int dashCount;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashLimit;
    [SerializeField] private float dashStrength;
    [SerializeField] private float dashDrag;
    private float currentDrag;

    private Vector2 smoothVelocity; // Empty velocity reference for all Smoothdamp functions.

    //// Movement Smoothdamp
    // private Vector2 currentInputVectorRotate;
    // [SerializeField] private float smoothRotate;

    // Dash Smoothdamp
    [SerializeField] private float brakeStrengthInverse; // Dash mechanic employs a brake to function. Strength decreases as field value increases. 100 is preferred.

    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip dashAudioClip;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float dashVolume;

    // Collision detection
    [SerializeField] private bool isColliding = false;

    [SerializeField] public int keyAmountCollected;
    [SerializeField] private bool isPlayerControlEnabled;
    
    private Rigidbody2D playerRigidbody2D;
    // private PlayerInput PlayerInput;
    public PlayerControls playerControls;
    
    void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        isMovementActive = true;
        currentMovementStrength = movementStrength;
        currentDrag = playerRigidbody2D.drag;
    }

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
            if (playerRigidbody2D.velocity.magnitude > minimumVelocityToControlPlayer)
            {
                playerControls.Player.Movement.Enable();
            }
            else
            {
                playerControls.Player.Movement.Disable();
                //StartCoroutine(PlayerMovementCheck());
            }

            // If the speed is below 0.5 but still not colliding, that means the player is stuck.
            // The PlayerUnlatch coroutine checks for this and unlatches the player.
            if (playerRigidbody2D.velocity.magnitude < 0.5f)
            {
                StartCoroutine(PlayerUnlatch());
            }

        }


        // If statement only for brake. Final build won't require the if wrapper.
        if (isMovementActive)
        {
            playerRigidbody2D.AddForce(transform.up.normalized * movementStrength * Time.deltaTime, ForceMode2D.Force);
            Debug.Log(playerRigidbody2D.velocity.magnitude);
        }
        else
        #endregion

        #region Brake [Only for testing]
        // Hold [CIRCLE]/[B] or SHIFT key to activate hard-brakes.
        // Stops player immediately.

        if (!isMovementActive)
        {
            OnBrake();
        }
        #endregion

        #region Dash
        // Tap [CROSS]/[A] or SPACE key to dash.

        if (playerControls.Player.Dash.IsPressed())
        {
            if (playerRigidbody2D.velocity.magnitude < 100)
            {
                playerControls.Player.Dash.Disable();
                StartCoroutine(DashCoroutine(brakeStrengthInverse));
            }
            else if (playerRigidbody2D.velocity.magnitude > 100)
            {
                OnBrake();
                playerControls.Player.Dash.Disable();
                StartCoroutine(DashCoroutine(brakeStrengthInverse));
            }
        }
        #endregion
    }

    void Update()
    {
        #region Brake [Only for testing]
        // Only used to get proper key presses and releases.

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
        #endregion

        #region Control Enable/Disable
        if (!isPlayerControlEnabled)
        {
            playerControls.Player.Disable();
        }
        #endregion
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
    
    // To brake.
    void OnBrake()
    {
        playerRigidbody2D.velocity = Vector2.zero;
        playerRigidbody2D.angularVelocity = 0;
    }

    // To perform a dash.
    IEnumerator DashCoroutine(float smoothTime)
    {
        playerAudioSource.PlayOneShot(dashAudioClip, dashVolume);
        dashCount++;

        // Apply a force
        playerRigidbody2D.drag = dashDrag;
        playerRigidbody2D.AddForce(transform.up.normalized * (dashStrength * 10) * Time.deltaTime, ForceMode2D.Impulse);
        
        // Duration of the force
        yield return new WaitForSeconds(dashDuration);
        
        // Make the player come to stop.
        movementStrength = 0;
        playerRigidbody2D.velocity = Vector2.SmoothDamp(playerRigidbody2D.velocity, Vector2.zero, ref smoothVelocity, (smoothTime / 1000));
        playerRigidbody2D.angularVelocity = 0;
        movementStrength = currentMovementStrength;
        playerRigidbody2D.drag = currentDrag;

        // Time before you can use dash again.
        yield return new WaitForSeconds(dashLimit);

        playerControls.Player.Dash.Enable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
        StopCoroutine(PlayerUnlatch());
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }
    IEnumerator PlayerUnlatch()
    {
        Debug.Log("Start coroutine");
        yield return new WaitForSeconds(2f);
        isColliding = true;
        yield return new WaitForSeconds(1f);
        isColliding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Used for key collection.
        if (collision.gameObject.tag == "Key")
        {
            keyAmountCollected++;
            Destroy(collision.gameObject);
        }
    }
}
