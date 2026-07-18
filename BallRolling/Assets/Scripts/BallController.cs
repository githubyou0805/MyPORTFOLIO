using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float moveSpeed = 20f;

    [Header("ジャンプ力")]
    [SerializeField] private float jumpForce = 8f;
    [Header("ジャンプ判定の勢い（閾値）")]
    [SerializeField] private float jumpThreshold = 1.8f; // この値より勢いよく手前に引くとジャンプ

    [SerializeField] private bool invertFrontBack = false; // 前後反転
    [SerializeField] private bool invertLeftRight = false; // 左右反転

    [SerializeField] private Transform RespawnPos;
    private Rigidbody rb;
    private bool isGrounded;
    private float lastZAcceleration = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }
    }

    void FixedUpdate()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // 地面接地判定（簡易的にY軸の速度や高さで判定）
        isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.01f;

        if (Accelerometer.current != null)
        {
            Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();

            // 左右の傾き（ハンドル操作）
            moveX = acceleration.x;

            // 前後の傾き（スマホのZ軸）
            moveZ = -acceleration.z;

            // 反転処理
            if (invertFrontBack) moveZ *= -1f;
            if (invertLeftRight) moveX *= -1f;

            // --- ジャンプ判定（勢いよく手前に傾ける） ---
            // 前回の値との差分（躍度・変化量）を計算
            float zChange = acceleration.z - lastZAcceleration;

            // 地面にいて、かつ変化量が閾値を超えた場合にジャンプ
            if (isGrounded && zChange > jumpThreshold)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            // 今回の値を保存
            lastZAcceleration = acceleration.z;
        }

        if (moveX == 0f && moveZ == 0f)
        {
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");

            // PCテスト用：スペースキーでジャンプ
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        // 移動ベクトルを作成してボールに力を加える
        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        rb.AddForce(movement * moveSpeed);
        if(transform.position.y < -20f)
        {
            transform.position = RespawnPos.position;
            rb.AddForce(-movement * moveSpeed);
        }
    }
}