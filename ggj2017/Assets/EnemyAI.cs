using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public float speed = 2.5f;

    public Transform player;

    private float distToPlayer;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate enemy to face player position
        transform.LookAt(player.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);

        distToPlayer = Vector3.Distance(transform.position, player.position);

        //enemy moves towards the player
        if (distToPlayer > 0.5f)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }
}
