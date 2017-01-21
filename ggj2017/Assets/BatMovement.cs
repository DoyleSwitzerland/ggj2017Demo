using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour {

    private float speed = 3.0f;

    private float topSpeed = 8.0f;

    private Rigidbody rb;

    private float inputHorzontal;

    private float inputVertical;

    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        inputHorzontal = Input.GetAxisRaw("Horizontal");

        inputVertical = Input.GetAxisRaw("Vertical");

        if(inputHorzontal == 0 && inputVertical == 0)
        {
            speed = 3.0f;
        }
        else
        {
            if(speed < topSpeed)
            {
                speed += Time.deltaTime*2;
            }
        }

        rb.velocity = (Vector3.right * inputHorzontal * speed) + (Vector3.up * inputVertical * speed);
	}
}
