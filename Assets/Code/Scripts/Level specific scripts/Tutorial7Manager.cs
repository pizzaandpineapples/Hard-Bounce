using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial7Manager : MonoBehaviour
{
    [SerializeField] private float weightLerpSpeed;
    private Vector2 smoothVelocity;

    [SerializeField] private GameObject redBarriers;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "ObstacleBouncy")
        {
            Destroy(redBarriers);
        }

        if (collision.gameObject.tag == "Weights")
        {
            Destroy(redBarriers);
            collision.transform.position = Vector2.SmoothDamp(collision.transform.position, transform.position, ref smoothVelocity,weightLerpSpeed * Time.deltaTime);
            collision.attachedRigidbody.velocity = Vector2.zero;
        }
    }
}
