using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5Handler : MonoBehaviour
{
    [SerializeField] private int dashCount;
    [SerializeField] private int howManyDashes;

    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject goThisWay;
    [SerializeField] private GameObject nextLevel;

    private Rigidbody2D playerRigidbody2D;
    [SerializeField] private float playerVelocity; 
    private PlayerControls playerControls;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity = playerRigidbody2D.velocity.magnitude;

        if (playerControls.Player.Dash.IsPressed())
        {
            Debug.Log("X is pressed");
            
        }
        if (playerControls.Player.Dash.WasReleasedThisFrame())
        {
            Debug.Log("X was released");
            dashCount++;
        }

        if (playerVelocity > 100)
        {
            instructions.gameObject.SetActive(true);
        }

        if (dashCount >= howManyDashes)
        {
            goThisWay.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }
}
