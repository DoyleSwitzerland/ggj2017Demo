using UnityEngine;

public class BatMovement : MonoBehaviour {

    public float minSpeed = 6.0f;
    public float topSpeed = 16.0f;
    public float accelerationFactor = 40f;

    private float inputHorizontal;
    private float inputVertical;

    private float speed;
    public float Speed {
        get {
            return speed;
        }
    }

    private bool isStunned;
    public bool IsStunned {
        get {
            return isStunned;
        }
    }

    // Use this for initialization
    void Start () {
        isStunned = false;
        speed = minSpeed;
	}
	
	// Update is called once per frame
	public Vector3 calculateVelocity () {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

        if(inputHorizontal == 0 && inputVertical == 0)
        {
            speed = minSpeed;
        }
        else
        {
            if(speed < topSpeed)
            {
                speed += Time.deltaTime * accelerationFactor;
            }
        } 
        
        return (Vector3.right * inputHorizontal * speed) + (Vector3.up * inputVertical * speed);
    }

}
