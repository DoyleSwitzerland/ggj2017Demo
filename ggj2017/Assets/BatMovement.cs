using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour {

    public float minSpeed = 3.0f;
    public float topSpeed = 8.0f;
    public float accelerationFactor = 5f;

    private Rigidbody rb;
    private float inputHorizontal;
    private float inputVertical;
    private float speed;
    private bool isStunned;

    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        isStunned = false;
        speed = minSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

        if(inputHorizontal == 0 && inputVertical == 0)
        {
            speed = minSpeed;
        }
        else
        {
            if(minSpeed < topSpeed)
            {
                speed += Time.deltaTime * accelerationFactor;
            }
        }
        if(!isStunned) {
            rb.velocity = (Vector3.right * inputHorizontal * speed) + (Vector3.up * inputVertical * speed);
        }        
	}

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Obstacle")) {
            isStunned = true;
            StartCoroutine(Stunned());
            // calculate force vector
            var force = transform.position - collision.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            rb.AddForce(force * 100);
        }
    }

    private IEnumerator Stunned() {
        yield return new WaitForSeconds(0.5f);
        isStunned = false;
    }
}
