using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float zRotation;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 0.0f, 1.0f * Time.deltaTime * zRotation);
    }
}