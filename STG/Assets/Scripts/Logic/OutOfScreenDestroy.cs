using UnityEngine;
using UnityEngine.UIElements;

public class OutOfScreenDestroy : MonoBehaviour
{
    private Camera mainCam;
    private float offset = 1f; // 少し余裕を持たせる

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 viewPos = mainCam.WorldToViewportPoint(pos);

        // 画面外判定（0〜1の範囲が画面内）
        if (viewPos.x < -offset || viewPos.x > 1 + offset ||
            viewPos.y < -offset || viewPos.y > 1 + offset)
        {
            Destroy(gameObject);
        }
    }
}
