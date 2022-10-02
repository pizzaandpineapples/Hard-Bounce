using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial7Handler : MonoBehaviour
{
    [SerializeField] private GameObject redWalls;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "ObstacleBouncy")
        {
            Destroy(redWalls);
        }
    }
}
