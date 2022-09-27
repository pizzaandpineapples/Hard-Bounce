using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3Event : MonoBehaviour
{
    [SerializeField] private int bounceCount;
    [SerializeField] private int howManyBouncesToNextLevel;

    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (bounceCount >= howManyBouncesToNextLevel)
        {
            goThisWayInstruction.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        bounceCount++;
    }
}
