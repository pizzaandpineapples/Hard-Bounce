using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    #region Enemy Pathfinding
    [SerializeField] private bool isUsingPathfinder = false;

    private Vector2 movement;
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker; // Handles path calls for a single unit. Basically generates paths.
    #endregion

    [SerializeField] private float detectionRadius;
    [SerializeField] private float weaponSpeed;
    private float weaponSpeedTimer;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private Transform ammoSpawnPosition; // A separate transform variable is used so that we can manually change the ammo spawn location as per our need.
    [SerializeField] private Transform aimCenter;

    [SerializeField] private Transform target; // Getting the Player's transform

    private Rigidbody2D EnemyRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRigidbody2D = GetComponent<Rigidbody2D>();

        #region Enemy Pathfinding
        if (isUsingPathfinder)
        {
            seeker = GetComponent<Seeker>();

            // Creates a path from the enemy to the player. Then calls a function once the path is created.
            InvokeRepeating("UpdatePath", 0f, 0.5f);
            // seeker.StartPath(EnemyRigidbody2D.position, target.position, OnPathComplete); // Moved to UpdatePath method below.
        }
        #endregion

        weaponSpeedTimer = Time.time;
    }

    void FixedUpdate()
    {
        #region Enemy Pathfinding
        if (isUsingPathfinder)
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
            float distanceToNextWaypoint = Vector2.Distance(EnemyRigidbody2D.position, path.vectorPath[currentWaypoint]);

            // If the distance less than the nextWaypointDistance, it means we have reached our next waypoint.
            // So we just increment our currentWaypoint.
            if (distanceToNextWaypoint < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        #endregion

        #region Fire Weapon Basic
        
        // Distance between enemy and player.
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        
        // Direction of the player.
        Vector3 directionOfTarget = (target.position - transform.position).normalized;

        // Angle of the player from the enemy.
        float aimAngleOffset = -90; // To correct the rotation, as this way of implementation creates an offset.
        float aimAngle = (Mathf.Atan2(directionOfTarget.y, directionOfTarget.x) * Mathf.Rad2Deg) + aimAngleOffset;
        aimCenter.transform.rotation = Quaternion.Euler(0, 0, aimAngle);

        // Weapon speed controller.
        weaponSpeedTimer += Time.deltaTime;
        // Debug.Log(weaponTimer);
        if (weaponSpeedTimer >= weaponSpeed)
        {
            // Fire when the target is within the detection radius
            if (distanceToTarget <= detectionRadius)
            {
                // Uses the transforms rotation to make the ammo rotate in the direction the player is aiming.
                Instantiate(ammoPrefab, ammoSpawnPosition.position, aimCenter.transform.rotation);
                // Debug.Log(weaponTimer);
                weaponSpeedTimer = 0;
            }
        }
        #endregion
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

    /* OLD ENEMY FOLLOW SYSTEM.
     * Works only when there are no obstacles or complex paths.

    // This section below was within the FixedUpdate method.
    MoveEnemy(movement);

    *****

    // This section below was within the Update method.
    // Direction
    Vector3 direction = target.position - transform.position;
    direction.Normalize();
    movement = direction;

    // Angle
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    EnemyRigidbody2D.rotation = angle;

    *****

    void MoveEnemy(Vector2 direction)
    {
        EnemyRigidbody2D.AddForce(direction * speed * Time.deltaTime);
    }
    */
}
