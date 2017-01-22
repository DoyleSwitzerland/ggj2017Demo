using System.Collections;
using UnityEngine;

public class BatController : MonoBehaviour {

    private AudioSource[] audioSources;
    private AudioSource flap;
    private AudioSource screech;
    private AudioSource injury;

    private BatMovement movement;
    private Animator animator;
    private EchoSource echoSource;
    private Rigidbody rb;
    private float numLives = 5.0f;
    public GameObject dedScreen;

    public OwlAI owl;

    private string IDLE = "Idle";
    private string FLYING = "Flying";
    private string IS_STUNNED = "IsStunned";
    private string IS_ECHOING = "IsEchoing";

    private bool isStunned;
    private bool canTakeDamage;
    public float stunTime = 1.0f;
    public float damageWaitTime = 1.5f;

    public float NumLives {
        get {
            return this.numLives;
        }

        set {
            this.numLives = value;
        }
    }

    public bool IsStunned {
        get { return isStunned; }
    }

    void Start() {
        canTakeDamage = true;

        audioSources = GetComponents<AudioSource>();
        flap = audioSources[0];
        screech = audioSources[1];
        injury = audioSources[2];

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        echoSource = GetComponent<EchoSource>();
        movement = GetComponent<BatMovement>();
    }

    void Update() {
        checkActions();
        checkAnimationState();

        if (numLives == 0)
        {
            dedScreen.SetActive(true);
            Time.timeScale = 0.0f;
            flap.Stop();
        }
    }

    private void checkActions() {
        if (!isStunned) {
            rb.velocity = movement.calculateVelocity();

            bool isFacingRight = movement.getDirection();
            updateDirection(isFacingRight);

            if (Input.GetAxisRaw("Fire1") == 1) {
                echoSource.CreateEcho(isFacingRight);
                owl.PlayerEchoed();
            }
        }
    }

    private void updateDirection(bool isFacingRight) {
        if (isFacingRight) {
            rb.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        } else {
            rb.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
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
            screech.Play();
            echoSource.IsEchoing = false;
        } else {
            animator.SetBool(IS_ECHOING, false);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        StartCoroutine(Stunned());
        injury.Play();
        var force = transform.position - collision.transform.position;
        force.Normalize();
        rb.AddForce(force * 100);
        isStunned = true;


        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hazard")) && canTakeDamage) {
            numLives--;
            StartCoroutine(CanTakeDamage());
            canTakeDamage = false;
        }

        if (collision.gameObject.CompareTag("Owl")) {
            numLives = 0;
        }

        if (collision.gameObject.CompareTag("Moth")) {
            numLives++;
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator Stunned() {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }

    private IEnumerator CanTakeDamage() {
        yield return new WaitForSeconds(damageWaitTime);
        canTakeDamage = true;
    }
}
