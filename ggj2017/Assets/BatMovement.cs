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

    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
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
                minSpeed += Time.deltaTime * accelerationFactor;
            }
        }

        rb.velocity = (Vector3.right * inputHorizontal * minSpeed) + (Vector3.up * inputVertical * minSpeed);
	}
}
