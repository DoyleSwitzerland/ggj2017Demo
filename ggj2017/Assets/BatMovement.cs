using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour {

    private float speed = 3.0f;

    private float topSpeed = 8.0f;

    private float accelerationFactor = 5f;

    private Rigidbody rb;

    private float inputHorizontal;

    private float inputVertical;

    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        inputHorizontal = Input.GetAxis("Horizontal");

        inputVertical = Input.GetAxis("Vertical");

        print(inputHorizontal);

        if(inputHorizontal == 0 && inputVertical == 0)
        {
            speed = 3.0f;
        }
        else
        {
            if(speed < topSpeed)
            {
                speed += Time.deltaTime * accelerationFactor;
            }
        }

        rb.velocity = (Vector3.right * inputHorizontal * speed) + (Vector3.up * inputVertical * speed);
	}
}
