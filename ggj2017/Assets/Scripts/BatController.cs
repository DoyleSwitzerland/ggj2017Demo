using System.Collections;
using UnityEngine;

public class BatController : MonoBehaviour {

    private BatMovement movement;
    private Animator animator;
    private EchoSource echoSource;
    private Rigidbody rb;
    private float numLives = 5.0f;


    private string IDLE = "Idle";
    private string FLYING = "Flying";
    private string IS_STUNNED = "IsStunned";
    private string IS_ECHOING = "IsEchoing";

    private bool isStunned;
    public float stunTime = 1.0f;

    public float NumLives 
    {
        get
        {
            return this.numLives;
        }

        set
        {
            this.numLives = value;
        }
    }

    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        echoSource = GetComponent<EchoSource>();
        movement = GetComponent<BatMovement>();
    }
	
	void Update () {
        checkActions();
        checkAnimationState();
    }

    private void checkActions() {
        if (!isStunned) {
            rb.velocity = movement.calculateVelocity();
            if (Input.GetAxisRaw("Fire1") == 1) {
                echoSource.CreateEcho();
            }
        }
    }

    private void checkAnimationState() {
        if (isStunned) {
            animator.SetBool(IS_STUNNED, true);
        } else {
            animator.SetBool(IS_STUNNED, false);
        }
        
        if (echoSource.IsEchoing) {
            animator.SetBool(IS_ECHOING, true);
            echoSource.IsEchoing = false;
        } else {
            animator.SetBool(IS_ECHOING, false);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Obstacle")) {
            StartCoroutine(Stunned());
            var force = transform.position - collision.transform.position;
            force.Normalize();
            rb.AddForce(force * 100);
            isStunned = true;
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            print("enemy hit");
            numLives--;
        }
    }

    private IEnumerator Stunned() {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }
}
