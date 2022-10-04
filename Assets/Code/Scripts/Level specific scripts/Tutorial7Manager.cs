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
        if (collision.gameObject.tag == "Weight" || collision.gameObject.tag == "WeightBouncy")
        {
            Vector2 direction = collision.attachedRigidbody.velocity.normalized;

            redBarriers.SetActive(false);
            collision.transform.position = Vector2.SmoothDamp(collision.transform.position, transform.position, ref smoothVelocity,weightLerpSpeed * Time.deltaTime);
            collision.transform.position = Vector2.SmoothDamp(collision.transform.position, transform.position, ref smoothVelocity, weightLerpSpeed * Time.deltaTime);
            //collision.attachedRigidbody.velocity = Vector2.zero;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weight" || collision.gameObject.tag == "WeightBouncy")
        {
            redBarriers.SetActive(true);
        }
    }
}
