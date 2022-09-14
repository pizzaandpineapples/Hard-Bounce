using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] maps;

    // Start is called before the first frame update
    void Start()
    {
        int animalIndex = Random.Range(0, maps.Length);
        Vector3 spawnPos = new Vector3(0, 0, 0);
        Vector3 rotation = new Vector3(0, 0, Random.Range(0, 361));

        Instantiate(maps[animalIndex], spawnPos, Quaternion.Euler(rotation));
    }
}
