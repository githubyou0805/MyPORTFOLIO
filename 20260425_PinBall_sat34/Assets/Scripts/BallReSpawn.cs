using UnityEngine;

public class BallReSpawn : MonoBehaviour
{
    public Transform plungerPoint; // Plunger �̊J�n�ʒu
    public Rigidbody ballRb;       // �{�[���� Rigidbody

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // ��������x�~�߂�
            ballRb.linearVelocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;

            // Plunger �̈ʒu�փ��[�v
            ballRb.transform.position = plungerPoint.position;

            // ������ɕ������Ė��܂�Ȃ��悤�ɂ���
            ballRb.transform.position += new Vector3(0, 0f, 0);
        }
    }
}
