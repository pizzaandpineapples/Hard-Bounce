using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1Handler : MonoBehaviour
{
    [SerializeField] private int bounceCount;
    [SerializeField] private int howManyBounces;

    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject goThisWay;
    [SerializeField] private GameObject nextLevel;

    // Start is called before the first frame update
    void Start()
    {
        howManyBounces = Random.Range(8, 13);
    }

    // Update is called once per frame
    void Update()
    {
        if (bounceCount >= howManyBounces)
        {
            goThisWay.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        bounceCount++;
    }
}