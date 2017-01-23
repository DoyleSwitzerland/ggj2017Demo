using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoPropagation : MonoBehaviour {
    public float lifeTime;
    public float maxSize;
    public float sizeIncreasePerFrame;
    public float minAspectRatio;
    public float aspectRatioDecreasePerFrame;
    public float ageToStopDisplaying = 0.01f;

    private float defaultLifeTime;
    private Projector echoProjector;
    private Rigidbody echoRigidBody;
    private Collider echoCollider;
    private SpriteRenderer echoSprite;

    public Rigidbody EchoRigidBody {
        get { return echoRigidBody; }
    }

    public Collider EchoCollider {
        get { return echoCollider; }
    }

    // Use this for initialization
    public void Setup(Transform parent, float echoSpeed) {
        defaultLifeTime = lifeTime;

        echoProjector = gameObject.GetComponent<Projector>();
        echoRigidBody = gameObject.GetComponent<Rigidbody>();
        echoCollider = gameObject.GetComponent<SphereCollider>();
        echoSprite = GetComponent<SpriteRenderer>();

        transform.position = new Vector3(parent.position.x, parent.position.y, parent.position.z + transform.position.z);
        EchoRigidBody.velocity = new Vector3(echoSpeed, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime  <= 0) {
            Destroy(gameObject);
        } else if (lifeTime <= defaultLifeTime - ageToStopDisplaying) {
            echoSprite.enabled = false;
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

    void OnTriggerEnter(Collider collider) {
        if (!collider.gameObject.CompareTag("Player")) {
            echoSprite.enabled = false;
            echoCollider.enabled = false;
            echoProjector.enabled = true;
            EchoRigidBody.velocity = Vector3.zero;
            EchoRigidBody.angularVelocity = Vector3.zero;
        }
    }
}
