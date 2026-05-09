using UnityEngine;

public class FlipperController : MonoBehaviour
{
    public float hitPower = 40f;     // ボールに与える力（強め）
    public float minSpeed = 1.0f;    // この速度以上で動いている時だけ弾く

    private Transform parent;
    private Vector3 lastPos;
    private Vector3 flipperVelocity;

    void Start()
    {
        parent = transform.parent;
        lastPos = parent.position;

        // すり抜け防止
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        // 親の移動速度（回転による移動量）を計算
        flipperVelocity = (parent.position - lastPos) / Time.deltaTime;
        lastPos = parent.position;
    }

    void OnCollisionStay(Collision col)
    {
        if (!col.collider.CompareTag("Ball")) return;

        Rigidbody ballRb = col.collider.GetComponent<Rigidbody>();
        if (ballRb == null) return;

        // フリッパーが動いている時だけ力を加える
        if (flipperVelocity.magnitude > minSpeed)
        {
            Vector3 forceDir = flipperVelocity.normalized;
            ballRb.AddForce(forceDir * hitPower, ForceMode.Impulse);
        }
    }
}
