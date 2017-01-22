using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    public Texture tex;
    public float lives = 5.0f;

    private float texWidth;
    private float texHeight;

	// Use this for initialization
	void Start () {
        texWidth = tex.width;
        texHeight = tex.height;
	}
	
	// Update is called once per frame
	void Update () {
		
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
