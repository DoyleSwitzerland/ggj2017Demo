using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoPropagation : MonoBehaviour {
    public float lifeTime;
    public float maxSize;
    public float sizeIncreasePerFrame;
    public float minAspectRatio;
    public float aspectRatioDecreasePerFrame;

    //private Light echoLight;
    private Projector echoProjector;
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
        //echoLight = gameObject.GetComponent<Light>();
        echoProjector = gameObject.GetComponent<Projector>();
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
        if (echoProjector.enabled) {
            if (echoProjector.orthographicSize < maxSize) {
                echoProjector.orthographicSize += sizeIncreasePerFrame;
            }
            if (echoProjector.aspectRatio > minAspectRatio) {
                echoProjector.aspectRatio -= aspectRatioDecreasePerFrame;
            }
        }
	}

    void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Player")) {
            //echoLight.enabled = true;
            echoCollider.enabled = false;
            echoProjector.enabled = true;
            EchoRigidBody.velocity = Vector3.zero;
            EchoRigidBody.angularVelocity = Vector3.zero;
        }
    }
}
