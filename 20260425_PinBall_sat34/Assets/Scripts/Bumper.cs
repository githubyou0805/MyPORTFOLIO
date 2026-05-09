using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float bounceForce = 20f;   // ボールに与える力
    public float upwardBoost = 2f;    // 少し上に跳ねさせる補正

    void OnCollisionEnter(Collision col)
    {
        if (!col.collider.CompareTag("Ball")) return;

        Rigidbody ballRb = col.collider.GetComponent<Rigidbody>();
        if (ballRb == null) return;

        // 衝突点から外側へ向かう方向を計算
        Vector3 dir = (col.contacts[0].point - transform.position).normalized;

        // ボールを外側へ弾く
        Vector3 force = dir * bounceForce;

        // 少し上方向に力を足すとピンボールらしくなる
        force += Vector3.up * upwardBoost;

        ballRb.AddForce(force, ForceMode.Impulse);
    }
}
