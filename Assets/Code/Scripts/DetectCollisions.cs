using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("Player Ammo") && other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        // Destroy ammunition when they hit obstacles or boundaries
        if ((gameObject.CompareTag("Player Ammo") || gameObject.CompareTag("Enemy Ammo")) && (other.CompareTag("Obstacle") || other.CompareTag("Boundary")))
        {
            Destroy(gameObject);
        }
    }
}
