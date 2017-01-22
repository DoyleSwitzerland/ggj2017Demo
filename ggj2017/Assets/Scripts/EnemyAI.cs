using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public float speed = 2.5f;
    public Transform player;
    public float followDistance = 5f;
    public float unFollowDistance = 7.5f;
    public Transform[] patrolPoints;
    public Path path;
    public Vector3 TargetPosition;
    public Seeker seeker;

    private float distToPlayer;
    private bool shouldFollow = false;
    private float repathRate = .5f;
    private float lastRepath = -9999;
    private float nextWaypointDistance = 1f;
    private int currentWaypoint = 0;
    private int patrolPointIndex = 0;

    // Use this for initialization
    void Start() {
        player = GameObject.FindWithTag("Player").transform;
        //Get a reference to the Seeker component we added earlier
        seeker = GetComponent<Seeker>();
        SimpleSmoothModifier smoothModifier = gameObject.AddComponent<SimpleSmoothModifier>();
        seeker.RegisterModifier(smoothModifier);
        TargetPosition = patrolPoints[patrolPointIndex].position;
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath(transform.position, TargetPosition, OnPathComplete);
    }

    public void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    public void Update() {
        distToPlayer = Vector3.Distance(player.position, transform.position);
        if (distToPlayer <= followDistance) {
            TargetPosition = player.position;
            shouldFollow = true;
        }

        if (distToPlayer >= unFollowDistance) {
            TargetPosition = patrolPoints[patrolPointIndex].position;
            shouldFollow = false;
        }

        if (Time.time - lastRepath > repathRate && seeker.IsDone()) {
            lastRepath = Time.time + Random.value * repathRate * 0.5f;
            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, TargetPosition, OnPathComplete);
        }
        if (path == null) {
            // We have no path to follow yet, so don't do anything
            return;
        }
        if (currentWaypoint > path.vectorPath.Count) return;
        if (currentWaypoint == path.vectorPath.Count) {
            if (!shouldFollow) {
                patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
                TargetPosition = patrolPoints[patrolPointIndex].position;
            }
            currentWaypoint++;
            return;
        }
        // Direction to the next waypoint
        Vector3 dir = path.vectorPath[currentWaypoint];
        dir.z = 0;
        Move(dir);
        // The commented line is equivalent to the one below, but the one that is used
        // is slightly faster since it does not have to calculate a square root
        if (Vector3.Distance(transform.position, dir) < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }

    void Move(Vector3 moveTo) {
        transform.position = Vector3.Lerp(transform.position, moveTo, speed * Time.deltaTime);
    }

    private IEnumerator findPath(Vector3 TargetPosition) {
        Path p = seeker.StartPath(transform.position, TargetPosition, OnPathComplete);
        yield return StartCoroutine(p.WaitForPath());
    }
}
