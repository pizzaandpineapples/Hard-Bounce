using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BounceOffObjects : MonoBehaviour
{
    // Bounce
    private Vector3 lastVelocity;
    private float speed;
    private Vector3 direction;
    public int bounceCount;

    public bool isPlayerDead = false;

    private Rigidbody2D playerRigidbody2D;
    private PlayerController playerController;
    private Collider2D playerCollider;
    private SpriteRenderer playerSpriteRenderer;
    private TrailRenderer playerTrailRenderer;
    
    // Audio controls
    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] bounceAudioClips;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float bounceVolume;
    //[SerializeField] private float platfromDestroyVolume;
    //[SerializeField] private AudioClip platfromDestroyAudioClip;
    //[Range(0.0f, 1.0f)]
    [SerializeField] private AudioClip[] playerDeathAudioClips;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float playerDeathVolume;
    
    // Particle system
    [SerializeField] private ParticleSystem playerDeathParticleSystem;

    void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerCollider = GetComponent<Collider2D>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerTrailRenderer = GetComponentInChildren<TrailRenderer>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        lastVelocity = playerRigidbody2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        speed = lastVelocity.magnitude;
        direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        bounceCount++;

        int bounceAudioClipsIndex = Random.Range(0, bounceAudioClips.Length);
        int playerDeathAudioClipsIndex = Random.Range(0, playerDeathAudioClips.Length);

        #region In-general bounce behavior
        // Bounce of boundaries, non-bouncy objects and regular platforms.
        if (collision.gameObject.tag == "Boundary" || collision.gameObject.tag == "ObstacleBouncy" || collision.gameObject.tag == "WeightBouncy" || collision.gameObject.tag == "RegularPlatform" || collision.gameObject.tag == "DestroyablePlatform")
        {
            playerAudioSource.PlayOneShot(bounceAudioClips[bounceAudioClipsIndex], bounceVolume);
            playerRigidbody2D.velocity = direction * Mathf.Max(speed, 0);
        }
        #endregion

        #region Speed Multiplier Platforms
        if (collision.gameObject.tag == "SMPlatform1") // X1.25
        {
            playerAudioSource.PlayOneShot(bounceAudioClips[bounceAudioClipsIndex], bounceVolume);
            playerRigidbody2D.velocity = direction * Mathf.Max(speed * 1.25f, 0);
        }
        if (collision.gameObject.tag == "SMPlatform2") // X1.5
        {
            playerAudioSource.PlayOneShot(bounceAudioClips[bounceAudioClipsIndex], bounceVolume);
            playerRigidbody2D.velocity = direction * Mathf.Max(speed * 1.5f, 0);
        }
        if (collision.gameObject.tag == "SMPlatform3") // X2
        {
            playerAudioSource.PlayOneShot(bounceAudioClips[bounceAudioClipsIndex], bounceVolume);
            playerRigidbody2D.velocity = direction * Mathf.Max(speed * 2.0f, 0);
        }
        #endregion

        #region Speed Divider Platforms
        if (collision.gameObject.tag == "SDPlatform1") // /2
        {
            playerAudioSource.PlayOneShot(bounceAudioClips[bounceAudioClipsIndex], bounceVolume);
            playerRigidbody2D.velocity = direction * Mathf.Max(speed / 2.0f, 0);
        }
        if (collision.gameObject.tag == "SDPlatform2") // /4
        {
            playerAudioSource.PlayOneShot(bounceAudioClips[bounceAudioClipsIndex], bounceVolume);
            playerRigidbody2D.velocity = direction * Mathf.Max(speed / 4.0f, 0);
        }
        #endregion

        #region Unfriendly Platforms

        //if (collision.gameObject.tag == "EPlatform") // Escape Platform. Gets stuck. Dash to escape within fixed time.
        //{
        //    RandomBounceSFX(0.2f);
        //    playerRigidbody2D.velocity = Vector2.zero;
        //    //PlayerRigidbody2D.Sleep();
        //    //gameObject.GetComponent<PlayerController>().enabled = false;
        //    //PlayerController.playerControls.Player.Disable();
        //}

        if (collision.gameObject.tag == "DPlatform") // Death Platform. Results in player death.
        {
            isPlayerDead = true;

            // Hides the gameobjects in question.
            playerSpriteRenderer.enabled = false;
            playerTrailRenderer.enabled = false;
            //collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            // Disables player movement and controls.
            playerRigidbody2D.velocity = Vector2.zero;
            playerRigidbody2D.angularVelocity = 0;
            playerRigidbody2D.Sleep();
            playerCollider.enabled = false;
            playerController.enabled = false;

            // Plays audio clips and particle effects.
            //playerAudioSource.PlayOneShot(bounceAudioClips[bounceAudioClipsIndex], bounceVolume);
            playerAudioSource.PlayOneShot(playerDeathAudioClips[playerDeathAudioClipsIndex], playerDeathVolume);
            playerDeathParticleSystem.Play();
            //playerAudioSource.PlayOneShot(destroyFX, 0.1f);
            //collision.gameObject.GetComponentInChildren<ParticleSystem>().Play();

            // Destroys gameobjects.
            StartCoroutine(DestroyCoroutine(collision));
        }
        #endregion
    }
    
    IEnumerator DestroyCoroutine(Collision2D collision)
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        //Destroy(collision.gameObject);
    }
}
