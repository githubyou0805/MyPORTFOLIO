using UnityEngine;

public class PlacedItem : MonoBehaviour
{
    private bool isDragging = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.IsPlaying) return;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    // 配置フェーズ中に右クリックしたら削除できるオマケ機能
    void OnMouseOver()
    {
        if (GameManager.Instance.IsPlaying) return;

        if (Input.GetMouseButtonDown(1)) // 右クリック
        {
            Destroy(gameObject);
        }
    }
}