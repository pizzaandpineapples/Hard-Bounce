using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    // Spaceship properties
    [SerializeField] private float thrusterPower;
    [SerializeField] private float thrusterSpeed;
    [SerializeField] private float regularSpeed;

    // Movement Smoothdamp
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float inputSpeed;

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

        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputVector, ref smoothInputVelocity, inputSpeed); // maxSpeed parameter is not used, because we know that the keyboard entry will be clamped to 1.

        if (playerInputActions.Player.Thrusters.IsPressed())
        {
            PlayeRigidbody2D.AddRelativeForce(currentInputVector * thrusterPower * thrusterSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(currentInputVector * regularSpeed * Time.deltaTime);
        }

        #endregion


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
