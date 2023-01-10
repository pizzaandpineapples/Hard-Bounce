using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestructionHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private GameManager gameManagerScript;

    [SerializeField] private float velocityRequiredToBreak = 10f;
    [SerializeField] private int hitCount;
    [SerializeField] private int hitsNeededForDestructionMin = 3;
    [SerializeField] private int hitsNeededForDestructionMax = 4;
    [SerializeField] private int hitsNeededForDestruction;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] objectBreakingAudioClips;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float objectBreakingVolume;

    // Platform Shaker
    private ObjectShakeHandler shaker;
    [SerializeField] private float shakeDuration = 1f;

    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        shaker = gameObject.GetComponent<ObjectShakeHandler>();

        hitsNeededForDestruction = Random.Range(hitsNeededForDestructionMin, hitsNeededForDestructionMax);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int objectBreakingAudioClipsIndex = Random.Range(0, objectBreakingAudioClips.Length);

        shaker.Shake(shakeDuration);

        if (gameManagerScript.playerVelocity > velocityRequiredToBreak)
            hitCount++;

        if (hitCount >= hitsNeededForDestruction)
        {
            audioSource.PlayOneShot(objectBreakingAudioClips[objectBreakingAudioClipsIndex], objectBreakingVolume);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<ObjectShakeHandler>().enabled = false;
            transform.position = new Vector2(0, -30);
            StartCoroutine(DestroyCoroutine());
        }
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
