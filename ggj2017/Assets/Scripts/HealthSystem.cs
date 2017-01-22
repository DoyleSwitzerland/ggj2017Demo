using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    public Texture tex;

    public GameObject player;
    BatController bat;
    private float lives;
    private float texWidth;
    private float texHeight;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        bat = player.GetComponent<BatController>();
        lives = bat.NumLives;
        texWidth = tex.width;
        texHeight = tex.height;
	}
	
	// Update is called once per frame
	void Update () {
        lives = bat.NumLives;
	}

    void OnGUI()
    {
        if (lives > 0)
        {
            Rect posRect = new Rect(50, 50, texWidth / 5 * lives, texHeight);
            Rect texRect = new Rect(0, 0, 1.0f / 5.0f * lives, 1.0f);
            GUI.DrawTextureWithTexCoords(posRect, tex, texRect);
        }
    }
}
