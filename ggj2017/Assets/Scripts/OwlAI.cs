using Pathfinding;
using System.Collections;
using UnityEngine;

public class OwlAI : MonoBehaviour {

    public Transform player;
    public Vector3 TargetPosition;
    public float speed = 7f;

    private Path path;
    private Seeker seeker;
    private float distToPlayer;
    private bool shouldFollow = false;
    private float repathRate = .5f;
    private float lastRepath = -9999;
    private float nextWaypointDistance = 1f;
    private int currentWaypoint = 0;
    private BatMovement bat;
    private BatController batControl;

    // Use this for initialization
    void Start() {
        player = GameObject.FindWithTag("Player").transform;
        bat = player.gameObject.GetComponent<BatMovement>();
        batControl = player.gameObject.GetComponent<BatController>();
        //Get a reference to the Seeker component we added earlier
        seeker = GetComponent<Seeker>();
        SimpleSmoothModifier smoothModifier = gameObject.AddComponent<SimpleSmoothModifier>();
        seeker.RegisterModifier(smoothModifier);
        TargetPosition = player.position;
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath(transform.position, TargetPosition, OnPathComplete);
    }

    // Update is called once per frame
    void Update() {
        distToPlayer = Vector3.Distance(player.position, transform.position);

        if (batControl.IsStunned) {
            if (Random.Range(0, 100) >= 5) {
                shouldFollow = true;
                StartCoroutine(FollowTime(3));
            }
        }

        if (bat.IsMoving) {
            if (Random.Range(0, 100) >= 30) {
                shouldFollow = true;
                StartCoroutine(FollowTime(2));
            }
        }

        //For occasional idle chase
        if (Random.Range(0, 100) >= 80) {
            shouldFollow = true;
            StartCoroutine(FollowTime(2));
        }

        if (shouldFollow) {
            TargetPosition = player.position;
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
                currentWaypoint++;
                return;
            }
            // Direction to the next waypoint
            Vector3 dir = path.vectorPath[currentWaypoint];
            dir.z = 0;
            checkDirection(dir);
            Move(dir);
            // The commented line is equivalent to the one below, but the one that is used
            // is slightly faster since it does not have to calculate a square root
            if (Vector3.Distance(transform.position, dir) < nextWaypointDistance) {
                currentWaypoint++;
                return;
            }
        }
        else {
            MoveRandom();
        }
    }

    public void PlayerEchoed() {
        shouldFollow = true;
        StartCoroutine(FollowTime(5));
    }

    private void MoveRandom() {

    }

    void Move(Vector3 moveTo) {
        transform.position = Vector3.Lerp(transform.position, moveTo, speed * Time.deltaTime);
    }

    private void checkDirection(Vector3 moveTo) {
        bool isFacingRight = true;
        if (moveTo.x < transform.position.x) {
            isFacingRight = false;
        } 

        if (isFacingRight) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        } else {
            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        }
    }

    IEnumerator FollowTime(int time) {
        yield return new WaitForSeconds(time);
        shouldFollow = false;
    }

    private IEnumerator findPath(Vector3 TargetPosition) {
        Path p = seeker.StartPath(transform.position, TargetPosition, OnPathComplete);
        yield return StartCoroutine(p.WaitForPath());
    }

    public void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }
}
