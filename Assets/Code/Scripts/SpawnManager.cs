using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacleCourses;

    // Start is called before the first frame update
    void Start()
    {
        int obstacleCourseIndex = Random.Range(0, obstacleCourses.Length);
        Vector3 spawnPos = new Vector3(0, 0, 0);
        Vector3 spawnRotation = new Vector3 (0, 0, Random.Range(-360, 361));

        Instantiate(obstacleCourses[obstacleCourseIndex], spawnPos, Quaternion.Euler(spawnRotation));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
