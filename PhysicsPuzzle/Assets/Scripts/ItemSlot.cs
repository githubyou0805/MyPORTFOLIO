using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private GameObject itemPrefab; // 配置したい板のプレハブ
    private GameObject currentSpawnedItem;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // 枠をクリックした瞬間、プレハブを生成してドラッグ状態にする
    public void OnPointerDown(PointerEventData eventData)
    {
        // ゲーム実行中なら配置できない
        if (GameManager.Instance.IsPlaying) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // アイテムを生成
        currentSpawnedItem = Instantiate(itemPrefab, mousePos, itemPrefab.transform.rotation);
    }

    // ドラッグ中、マウスに追従させる
    public void OnDrag(PointerEventData eventData)
    {
        if (currentSpawnedItem == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        currentSpawnedItem.transform.position = mousePos;
    }

    // マウスを離したら配置確定
    public void OnPointerUp(PointerEventData eventData)
    {
        currentSpawnedItem = null;
        // ※必要に応じて、枠（スロット）のストック数をマイナス1する処理などをここに入れます
    }
}