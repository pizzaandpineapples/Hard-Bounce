using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerSpawnPosition;

    void Awake()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Vector3 rotation = new Vector3(0, 0, Random.Range(0, 361));
        Instantiate(player, playerSpawnPosition.position, Quaternion.Euler(rotation));
    }
}
