using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    private Vector2 movement;
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    [SerializeField] private Transform target; // Getting the Player's transform

    private Seeker seeker; // Handles path calls for a single unit. Basically generates paths.
    private Rigidbody2D EnemyRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        EnemyRigidbody2D = GetComponent<Rigidbody2D>();

        // Creates a path from the enemy to the player. Then calls a function once the path is created.
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        // seeker.StartPath(EnemyRigidbody2D.position, target.position, OnPathComplete); // Moved to UpdatePath method below.
    }

    void FixedUpdate()
    {
        // If there is no path, then we exit the function.
        if (path == null)
            return;

        // The currentWaypoint being greater or equals to means we have reached the end of the path or crossed it.
        // So reachedEndofPath is set as true.
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Direction between currentWaypoint on the path and the enemy.
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - EnemyRigidbody2D.position).normalized;

        // The force that we want to apply on the enemy
        Vector2 force = direction * speed * Time.deltaTime;
        EnemyRigidbody2D.AddForce(force);

        // Distance to the next waypoint (Enemy is the current position, and the currentWaypoint is the "next waypoint").
        float distance = Vector2.Distance(EnemyRigidbody2D.position, path.vectorPath[currentWaypoint]);

        // If the distance less than the nextWaypointDistance, it means we have reached our next waypoint.
        // So we just increment our currentWaypoint.
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Old Enemy follow system.
        /*

        MoveEnemy(movement);

        // This section was within the update method.
        // Direction
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        movement = direction;

        // Angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        EnemyRigidbody2D.rotation = angle;
        */
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(EnemyRigidbody2D.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    /* Old method to add force to enemy.
    void MoveEnemy(Vector2 direction)
    {
        EnemyRigidbody2D.AddForce(direction * speed * Time.deltaTime);
    }
    */
}
