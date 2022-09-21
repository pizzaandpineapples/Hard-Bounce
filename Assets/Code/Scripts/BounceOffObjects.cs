using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffObjects : MonoBehaviour
{
    private Vector3 lastVelocity;

    private PlayerController PlayerController;
    private Rigidbody2D PlayerRigidbody2D;

    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] BounceFX;
    [SerializeField] private AudioClip DestroyFX;
    [SerializeField] private AudioClip PopFX;

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
        int bounceIndex = Random.Range(0, BounceFX.Length);

        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        if (collision.gameObject.tag == "Boundary")
        {
            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed, 0);
        }
        if (collision.gameObject.tag == "RegularPlatform")
        {
            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed, 0);
        }

        // Speed Multiplier platforms
        if (collision.gameObject.tag == "SMPlatform1")
        {
            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 1.25f, 0);
        }
        if (collision.gameObject.tag == "SMPlatform2")
        {
            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 1.5f, 0);
        }
        if (collision.gameObject.tag == "SMPlatform3")
        {
            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 2.0f, 0);
        }

        // Speed Divider platforms
        if (collision.gameObject.tag == "SDPlatform1")
        {
            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed / 2.0f, 0);
        }
        if (collision.gameObject.tag == "SDPlatform2")
        {
            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed / 4.0f, 0);
        }
        if (collision.gameObject.tag == "SDPlatform3")
        {
            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
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

            playerAudioSource.PlayOneShot(BounceFX[bounceIndex], 0.2f);
            PlayerRigidbody2D.velocity = Vector2.zero;
            PlayerRigidbody2D.Sleep();
            gameObject.GetComponent<PlayerController>().enabled = false;
            PlayerController.playerControls.Player.Disable();

            playerAudioSource.PlayOneShot(DestroyFX, 0.1f);
            playerAudioSource.PlayOneShot(PopFX, 0.5f);

            StartCoroutine(DestroyCoroutine(collision));
        }
    }

    IEnumerator DestroyCoroutine(Collision2D collision)
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        Destroy(collision.gameObject);
    }
}
