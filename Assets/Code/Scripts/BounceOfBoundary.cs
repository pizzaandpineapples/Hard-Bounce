using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOfBoundary : MonoBehaviour
{
    private Vector3 lastVelocity;

    private Rigidbody2D PlayerRigidbody2D;

    void Awake()
    {
        PlayerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = PlayerRigidbody2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        PlayerRigidbody2D.velocity = direction * Mathf.Max(speed, 0);
    }
}
