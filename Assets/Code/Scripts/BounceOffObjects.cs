using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffObjects : MonoBehaviour
{
    private Vector3 lastVelocity;

    private PlayerController PlayerController;
    private Rigidbody2D PlayerRigidbody2D;

    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] bounceFX;
    [SerializeField] private AudioClip destroyFX;
    [SerializeField] private AudioClip popFX;

    [SerializeField] private ParticleSystem skullParticle;
    [SerializeField] private ParticleSystem explosionParticle;

    void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
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
        int bounceIndex = Random.Range(0, bounceFX.Length);

        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        if (collision.gameObject.tag == "Boundary" || collision.gameObject.tag == "RegularPlatform" || collision.gameObject.tag == "ObstacleBouncy")
        {
            playerAudioSource.PlayOneShot(bounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed, 0);
        }

        // Speed Multiplier platforms
        if (collision.gameObject.tag == "SMPlatform1")
        {
            playerAudioSource.PlayOneShot(bounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 1.25f, 0);
        }
        if (collision.gameObject.tag == "SMPlatform2")
        {
            playerAudioSource.PlayOneShot(bounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 1.5f, 0);
        }
        if (collision.gameObject.tag == "SMPlatform3")
        {
            playerAudioSource.PlayOneShot(bounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 2.0f, 0);
        }

        // Speed Divider platforms
        if (collision.gameObject.tag == "SDPlatform1")
        {
            playerAudioSource.PlayOneShot(bounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed / 2.0f, 0);
        }
        if (collision.gameObject.tag == "SDPlatform2")
        {
            playerAudioSource.PlayOneShot(bounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed / 4.0f, 0);
        }
        if (collision.gameObject.tag == "SDPlatform3")
        {
            playerAudioSource.PlayOneShot(bounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = Vector2.zero;
            PlayerRigidbody2D.Sleep();
            gameObject.GetComponent<PlayerController>().enabled = false;
            PlayerController.playerControls.Player.Disable();
        }

        // Trap platforms
        if (collision.gameObject.tag == "DPlatform")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            playerAudioSource.PlayOneShot(bounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = Vector2.zero;
            PlayerRigidbody2D.Sleep();
            gameObject.GetComponent<PlayerController>().enabled = false;
            PlayerController.playerControls.Player.Disable();

            playerAudioSource.PlayOneShot(destroyFX, 0.1f);
            playerAudioSource.PlayOneShot(popFX, 0.5f);
            skullParticle.Play();
            explosionParticle.Play();

            StartCoroutine(DestroyCoroutine(collision));
        }
    }

    IEnumerator DestroyCoroutine(Collision2D collision)
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        Destroy(collision.gameObject);
    }
}
