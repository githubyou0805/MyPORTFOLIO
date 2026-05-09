using UnityEngine;

public class Flipper : MonoBehaviour
{
    public KeyCode key = KeyCode.LeftArrow;

    public float restAngle = 0f;
    public float pressedAngle = 45f;
    public float rotateSpeed = 20f;
    public Vector3 rotateAxis = Vector3.forward;

    private Quaternion restRot;
    private Quaternion pressedRot;

    void Start()
    {
        restRot = Quaternion.Euler(rotateAxis * restAngle);
        pressedRot = Quaternion.Euler(rotateAxis * pressedAngle);
    }

    void Update()
    {
        if (Input.GetKey(key))
        {
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                pressedRot,
                Time.deltaTime * rotateSpeed
            );
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                restRot,
                Time.deltaTime * rotateSpeed
            );
        }
    }
}
