using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    // Spaceship properties
    [SerializeField] private float thrusterPower;
    [SerializeField] private float thrusterSpeed;
    [SerializeField] private float regularSpeed;

    // Movement Smoothdamp
    private Vector2 currentInputVectorMovement;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float inputSpeedMovement;

    // Rotate Smoothdamp
    private Vector2 currentInputVectorRotate;
    [SerializeField] private float inputSpeedRotate;

    // 
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region PlayerMovement

        Vector2 inputVectorMovement = playerInputActions.Player.Movement.ReadValue<Vector2>();
        // maxSpeed parameter is not used, because we know that the keyboard entry will be clamped to 1.
        // currentVelocity -> ref smoothInputVelocity. We don't need the current velocity, but we need to pass it.
        currentInputVectorMovement = Vector2.SmoothDamp(currentInputVectorMovement , inputVectorMovement, ref smoothInputVelocity, inputSpeedMovement);
        
        Vector2 inputVectorRotate = playerInputActions.Player.Look.ReadValue<Vector2>();
        currentInputVectorRotate = Vector2.SmoothDamp(currentInputVectorRotate, inputVectorRotate, ref smoothInputVelocity, inputSpeedRotate);

        // Will stay in last rotated position.
        if (inputVectorRotate != Vector2.zero)
        {
            RotateAim(currentInputVectorRotate.normalized);
        }
        // Only moves when thrusters are activated.
        //if (playerInputActions.Player.Thrusters.IsPressed())
        //{
        //    PlayeRigidbody2D.AddForce(currentInputVectorMovement * thrusterPower * thrusterSpeed * Time.deltaTime);
        //}
        PlayeRigidbody2D.AddForce(currentInputVectorMovement * thrusterPower * thrusterSpeed * Time.deltaTime);

        #endregion


    }
    public void RotateAim(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
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
