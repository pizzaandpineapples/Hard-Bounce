using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    private Vector2 movement;
    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform player; // Getting the Player's transform

    private Rigidbody2D EnemyRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        MoveEnemy(movement);
    }

    // Update is called once per frame
    void Update()
    {
        // Direction
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;

        // Angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        EnemyRigidbody2D.rotation = angle;
    }

    void MoveEnemy(Vector2 direction)
    {
        EnemyRigidbody2D.AddForce(direction * moveSpeed * Time.deltaTime);
    }
}
