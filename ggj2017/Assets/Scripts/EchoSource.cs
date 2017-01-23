using UnityEngine;

public class EchoSource : MonoBehaviour {
    public EchoPropagation baseEchoPropagation;
    public float coolDown;
    public float echoSpeed;

    private bool isEchoing;

    private float recharge;

    public bool IsEchoing {
        get {
            return isEchoing;
        }
        set {
            isEchoing = value;
        }
    }

    void Start () {
        recharge = coolDown;
        IsEchoing = false;
    }

    void Update() {
        recharge += Time.deltaTime*100;
    }

    public void CreateEcho(bool facingRight) {
        if (recharge >= coolDown) {
            recharge = 0;
            EchoPropagation echoPropagation = Instantiate<EchoPropagation>(baseEchoPropagation);
            echoPropagation.Setup(transform, echoSpeed, facingRight);
            IsEchoing = true;
        }
    }
}
