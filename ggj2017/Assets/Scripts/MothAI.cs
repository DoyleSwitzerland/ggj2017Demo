using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothAI : MonoBehaviour {

    public float speed = 1f;
    public float XScale = 1f;
    public float YScale = 1f;

    private Vector3 pivot;
    private Vector3 pivotOffset;
    private float phase;
    private bool invert = false;
    private const float twoPI = Mathf.PI * 2;

	// Use this for initialization
	void Start () {
        pivot = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        pivotOffset = Vector3.up * 2 * YScale;

        phase += speed * YScale;
        if (phase > twoPI) {
            invert = !invert;
            phase -= twoPI;
        }
        if (phase < 0) {
            phase += twoPI;
        }

        transform.position = pivot + (invert ? pivotOffset : Vector3.zero);
        Vector3 newPos = new Vector3(Mathf.Sin(phase) * XScale, Mathf.Cos(phase) * (invert ? -1 : 1) * YScale, 0);
        transform.position += newPos;
	}
}
