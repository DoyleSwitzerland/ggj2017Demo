using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoPropagation : MonoBehaviour {
    public float lifeTime;

    private Light echoLight;
    private Rigidbody echoRigidBody;
    private Collider echoCollider;

    public Rigidbody EchoRigidBody {
        get { return echoRigidBody; }
    }

    public Collider EchoCollider {
        get { return echoCollider; }
    }

    // Use this for initialization
    public void Setup(Transform parent, float echoSpeed) {
        echoLight = gameObject.GetComponent<Light>();
        echoRigidBody = gameObject.GetComponent<Rigidbody>();
        echoCollider = gameObject.GetComponent<SphereCollider>();

        transform.position = new Vector3(parent.position.x, parent.position.y, parent.position.z + transform.position.z);
        EchoRigidBody.velocity = new Vector3(echoSpeed, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime  <= 0) {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Player")) {
            print("Collided with " + collision.gameObject.name);
            echoLight.enabled = true;
            EchoRigidBody.velocity = Vector3.zero;
            print("setting " + EchoRigidBody + " velocity to " + EchoRigidBody.velocity);
        }
    }
}
