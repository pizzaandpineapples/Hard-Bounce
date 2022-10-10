using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform[] spawnPositions;

    // Start is called before the first frame update
    void Start()
    {
        int spawnPositionsIndex = Random.Range(0, spawnPositions.Length);
        Instantiate(objectToSpawn, spawnPositions[spawnPositionsIndex].position, spawnPositions[spawnPositionsIndex].rotation);
    }
}
