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
    public Path Path;
    public Vector3 TargetPosition;
    public Seeker seeker;

    private float distToPlayer;
    private bool shouldFollow = false;
    private float waitTime = 0.5f;
    private float time = 0;
    private const float nextWaypointDistance = .5f;
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

    // Update is called once per frame
    void Update() {
        distToPlayer = Vector3.Distance(player.position, transform.position);
        if (distToPlayer <= followDistance) {
            shouldFollow = true;
            TargetPosition = player.position;
        }

        if (shouldFollow && distToPlayer >= unFollowDistance) {
            shouldFollow = false;
            TargetPosition = patrolPoints[patrolPointIndex].position;
        }

        time += Time.deltaTime;
        if (time >= waitTime) {
            StartCoroutine(findPath(TargetPosition));
            time = 0;
        }

        if (Path != null) {
            //End of path reached
            if (currentWaypoint >= Path.vectorPath.Count) {
                patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
                currentWaypoint = 0;
                time += Time.deltaTime;
                if (time >= waitTime) {
                    StartCoroutine(findPath(TargetPosition));
                    time = 0;
                }
                return;
            }

            //TODO see if these three lines should be moved to the bottom of the update method, since logically it would make sense to move after the path is definitely set correctly
            Vector3 moveTo = Path.vectorPath[currentWaypoint];
            moveTo.z = 0;
            Move(moveTo);

            //Check if we are close enough to the next waypoint, if so proceed to follow the next waypoint
            if (Vector3.Distance(transform.position, Path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
                currentWaypoint++;
                return;
            }
        }
    }

    void Move(Vector3 moveTo) {
        transform.LookAt(moveTo);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            Path = p;
            currentWaypoint = 1;
        }
        seeker.StartPath(transform.position, TargetPosition, OnPathComplete);
    }

    private IEnumerator findPath(Vector3 TargetPosition) {
        Path p = seeker.StartPath(transform.position, TargetPosition, OnPathComplete);
        yield return StartCoroutine(p.WaitForPath());
    }
}
