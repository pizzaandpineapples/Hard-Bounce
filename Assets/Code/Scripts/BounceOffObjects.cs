using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffObjects : MonoBehaviour
{
    private Vector3 lastVelocity;
    private float speed;
    private Vector3 direction;

    private PlayerController PlayerController;
    private Rigidbody2D PlayerRigidbody2D;

    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] bounceFX;
    [SerializeField] private AudioClip destroyFX;
    [SerializeField] private AudioClip popFX;

    [SerializeField] private ParticleSystem skullParticle;

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
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        #region General Bounce
        // Bounce of boundaries, non-bouncy objects and regular platforms.
        if (collision.gameObject.tag == "Boundary" || collision.gameObject.tag == "ObstacleBouncy" || collision.gameObject.tag == "RegularPlatform")
        {
            RandomBounceSFX(0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed, 0);
            //StartCoroutine(PlatformUnlatchCoroutine(0.1f, collision));
        }
        #endregion

        #region Speed Multiplier Platforms
        // Speed Multiplier platforms
        if (collision.gameObject.tag == "SMPlatform1") // X1.25
        {
            RandomBounceSFX(0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 1.25f, 0);
        }
        if (collision.gameObject.tag == "SMPlatform2") // X1.5
        {
            RandomBounceSFX(0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 1.5f, 0);
        }
        if (collision.gameObject.tag == "SMPlatform3") // X2
        {
            RandomBounceSFX(0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed * 2.0f, 0);
        }
        #endregion

        #region Speed Divider Platform
        // Speed Divider platforms
        if (collision.gameObject.tag == "SDPlatform1") // /2
        {
            RandomBounceSFX(0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed / 2.0f, 0);
            //StartCoroutine(PlatformUnlatchCoroutine(0.2f, collision));
        }
        if (collision.gameObject.tag == "SDPlatform2") // /4
        {
            RandomBounceSFX(0.2f);
            PlayerRigidbody2D.velocity = direction * Mathf.Max(speed / 4.0f, 0);
            //StartCoroutine(PlatformUnlatchCoroutine(0.2f, collision));
        }
        #endregion

        #region Trap Platforms
        // Trap platforms
        if (collision.gameObject.tag == "EPlatform") // Escape Platform. Gets stuck. Dash to escape within fixed time.
        {
            RandomBounceSFX(0.2f);
            PlayerRigidbody2D.velocity = Vector2.zero;
            //PlayerRigidbody2D.Sleep();
            //gameObject.GetComponent<PlayerController>().enabled = false;
            //PlayerController.playerControls.Player.Disable();
        }
        if (collision.gameObject.tag == "DPlatform") // Death Platform. Gets destroyed.
        {
            // Hides the gameobjects in question.
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            // Disables player movement and controls.
            RandomBounceSFX(0.2f);
            PlayerRigidbody2D.velocity = Vector2.zero;
            PlayerRigidbody2D.Sleep();
            gameObject.GetComponent<PlayerController>().enabled = false;
            PlayerController.playerControls.Player.Disable();

            // Plays audio clips and particle effects.
            playerAudioSource.PlayOneShot(destroyFX, 0.1f);
            playerAudioSource.PlayOneShot(popFX, 0.5f);
            skullParticle.Play();
            collision.gameObject.GetComponentInChildren<ParticleSystem>().Play();
            //explosionParticle.Play();

            // Destroys both gameobjects.
            StartCoroutine(DestroyCoroutine(collision));
        }
        #endregion
    }

    void RandomBounceSFX(float sfxVolume)
    {
        int bounceIndex = Random.Range(0, bounceFX.Length);
        playerAudioSource.PlayOneShot(bounceFX[bounceIndex], sfxVolume);
    }

    IEnumerator PlatformUnlatchCoroutine(float time, Collision2D collision)
    {
        yield return new WaitForSeconds(time);
        PlayerRigidbody2D.AddForce(direction * 0.5f, ForceMode2D.Impulse); 
        collision.gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(time);
        collision.gameObject.GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator DestroyCoroutine(Collision2D collision)
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        Destroy(collision.gameObject);
    }
}
