using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1Event : MonoBehaviour
{
    [SerializeField] private int bounceCount;
    [SerializeField] private int howManyBouncesToNextLevel;

    [SerializeField] private GameObject controllerInstruction;
    [SerializeField] private GameObject goThisWayInstruction;
    [SerializeField] private GameObject nextLevel;

    // Start is called before the first frame update
    void Start()
    {
        howManyBouncesToNextLevel = Random.Range(6, 11);
        StartCoroutine(ControllerTextCoroutine());
    }

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

    IEnumerator ControllerTextCoroutine()
    {
        yield return new WaitForSeconds(10f);
        controllerInstruction.gameObject.SetActive(false);
    }
}
