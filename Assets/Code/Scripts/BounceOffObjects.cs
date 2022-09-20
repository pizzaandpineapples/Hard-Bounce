using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffObjects : MonoBehaviour
{
    private Vector3 lastVelocity;

    private Rigidbody2D PlayerRigidbody2D;

    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] BounceFX;

    void Awake()
    {
        PlayerRigidbody2D = GetComponent<Rigidbody2D>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = PlayerRigidbody2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int bounceIndex = Random.Range(0, BounceFX.Length);
        playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.3f);

        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        PlayerRigidbody2D.velocity = direction * Mathf.Max(speed, 0);
    }
}
