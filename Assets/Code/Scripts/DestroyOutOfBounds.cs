using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // Primarily for use with ammunition.

    private float verticalLimits = 12;
    private float horizontalLimits = 22;

    // Update is called once per frame
    void Update()
    {
        // Vertical Boundary
        if (transform.position.y > verticalLimits || transform.position.y < -verticalLimits)
        {
            Destroy(gameObject);
        }
        // Horizontal Boundary
        else if (transform.position.x > horizontalLimits|| transform.position.x < -horizontalLimits)
        {
            Destroy(gameObject);
        }
    }
}
