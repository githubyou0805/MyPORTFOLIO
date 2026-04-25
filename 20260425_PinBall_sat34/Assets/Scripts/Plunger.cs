using UnityEngine;
using UnityEngine.InputSystem;

public class Plunger : MonoBehaviour
{
    public float maxForce = 2000f;
    public float chargeSpeed = 10f;

    private float currentForce = 0f;
    private bool isBallReady = false;
    private Rigidbody ballRb;

    void Update()
    {
        if (isBallReady)
        {
            // キーボードのスペースキーの状態を取得
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // キーを押している間
            if (keyboard.spaceKey.isPressed)
            {
                if (currentForce < maxForce)
                {
                    currentForce += chargeSpeed * Time.deltaTime * 500f;
                }
            }

            // キーを離した瞬間
            if (keyboard.spaceKey.wasReleasedThisFrame)
            {
                LaunchBall();
            }
        }
    }

    private void LaunchBall()
    {
        if (ballRb != null)
        {
            ballRb.AddForce(Vector3.forward * currentForce);
            currentForce = 0f;
            isBallReady = false;
            ballRb = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            ballRb = collision.gameObject.GetComponent<Rigidbody>();
            isBallReady = true;
        }
    }
}
