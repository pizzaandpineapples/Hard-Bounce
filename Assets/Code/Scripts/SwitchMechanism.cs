using System;
using DG.Tweening;
using UnityEngine;

public class SwitchMechanism : MonoBehaviour
{
    [SerializeField] private float moveTweenDuration = 1.5f;
    [SerializeField] private float easeOutBackOvershoot = 1.70158f;

    public static event Action OnSwitchDown;
    public static event Action OnSwitchUp;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weight" || collision.gameObject.tag == "WeightBouncy")
        {
            collision.transform.DOMove(transform.position, moveTweenDuration).SetEase(Ease.OutBack, easeOutBackOvershoot);
            collision.attachedRigidbody.velocity = Vector2.zero;

            OnSwitchDown?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weight" || collision.gameObject.tag == "WeightBouncy")
        {
            OnSwitchUp?.Invoke();
        }
    }



    /* TODO: Potential idea of a glitchy teleport
    IEnumerator SwitchActivationCoroutine(Collider2D collision)
    {
        Vector2 direction = collision.attachedRigidbody.velocity.normalized;
        collision.transform.position = Vector2.SmoothDamp(collision.transform.position, (Vector2)transform.position + (direction * Vector2.Distance((Vector2)transform.position, new Vector2(0.01f, 0.01f))), ref smoothVelocity, weightLerpSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        collision.transform.position = Vector2.SmoothDamp(collision.transform.position, transform.position, ref smoothVelocity, weightLerpSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<Collider2D>().enabled = true;
    } */
}
