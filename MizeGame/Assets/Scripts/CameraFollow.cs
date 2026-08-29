using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera")]
    [SerializeField] private float zPosition = -10f;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            zPosition
        );
    }

    /// <summary>
    /// カメラの追従対象を設定
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}