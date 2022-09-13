using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] ObstacleCourse;

    // Start is called before the first frame update
    void Start()
    {
        int animalIndex = Random.Range(0, ObstacleCourse.Length);
        Vector3 spawnPos = new Vector3(0, 0, 0);
        Vector3 rotation = new Vector3(0, 0, Random.Range(-360, 361));
        Instantiate(ObstacleCourse[animalIndex], spawnPos, Quaternion.Euler(rotation));
    }
}
