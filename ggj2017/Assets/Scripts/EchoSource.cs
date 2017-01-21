using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoSource : MonoBehaviour {
    public EchoPropagation baseEchoPropagation;
    public float coolDown;
    public float echoSpeed;

    public Texture echoTexture;

    public float recharge;

	// Use this for initialization
	void Start () {
        recharge = coolDown;
        //GetComponent<Projector>().material.SetTexture("_ShadowTex", echoTexture);
    }

    void Update() {
        recharge += Time.deltaTime*100;
        if (recharge >= coolDown && Input.GetAxisRaw("Fire1") == 1) {
            CreateEcho();
            recharge = 0;
        }
    }

    void CreateEcho() {
        
    }

    void CreateEchoOld() {
        EchoPropagation echoPropagation = Instantiate<EchoPropagation>(baseEchoPropagation);
        echoPropagation.Setup(transform, echoSpeed);
    }


}
