using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShakeHandler : MonoBehaviour
{
    private Transform target;
    private Vector3 initialPosition;

    private bool isShaking = false;
    private float pendingShakeDuration = 0f;
    [SerializeField] private float intensity;

    void Start()
    {
        target = GetComponent<Transform>();
        initialPosition = target.localPosition;
    }

    void Update()
    {
        if (pendingShakeDuration > 0 && !isShaking)
        {
            StartCoroutine(DoShake());
        }
    }

    public void Shake(float duration)
    {
        if (duration > 0)
        {
            pendingShakeDuration += duration;
        }
    }

    IEnumerator DoShake()
    {
        isShaking = true;

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + pendingShakeDuration)
        {
            Vector3 randomPoint = new Vector3(Random.Range(-1f, 1f) * intensity, Random.Range(-1f, 1f) * intensity, initialPosition.z );
            target.localPosition = target.localPosition + randomPoint;
            yield return null;
        }

        pendingShakeDuration = 0f;
        target.localPosition = initialPosition;
        isShaking = false;
    }
}
