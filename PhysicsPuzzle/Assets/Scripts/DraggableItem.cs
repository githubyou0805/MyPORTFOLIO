using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    [Header("サイズ設定")]
    [SerializeField] private Vector3 placedScale = new Vector3(1f, 1f, 1f); // ステージ配置時のサイズ
    [SerializeField] private Vector3 slotScale = new Vector3(0.4f, 0.4f, 1f); // 枠の中にいる時のサイズ

    private Camera mainCamera;
    private Vector3 startPosition; // 枠の中の初期位置
    private bool isDragging = false;
    private int overlapCount = 0;  // 接触しているオブジェクトの数

    private Rigidbody2D rb;

    void Start()
    {
        mainCamera = Camera.main;
        startPosition = transform.position; // 最初の枠の位置を記憶
        rb = GetComponent<Rigidbody2D>();

        // 最初は枠の中にいるので、縮小サイズにする
        transform.localScale = slotScale;
    }

    void Update()
    {
        // ドラッグ中の処理（マウス位置に追従）
        if (isDragging)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;

            // リアルタイムサイズ変更: 枠の上にいるかどうかでサイズを切り替える
            if (IsOverSlot(mousePos))
            {
                transform.localScale = slotScale;
            }
            else
            {
                transform.localScale = placedScale;
            }

            // マウスを離したら配置チェック
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                CheckPlacement(mousePos);
            }
        }
    }

    // マウスがこのオブジェクトを直接クリックした時だけ呼ばれる（複数アイテム対応）
    void OnMouseDown()
    {
        // ゲーム実行中なら動かせない
        if (GameManager.Instance.IsPlaying) return;

        isDragging = true;
    }

    // 配置できるかどうかの判定
    private void CheckPlacement(Vector3 releasePos)
    {
        // 判定A: 枠（スロット）の中に置こうとしたか？
        bool isOnSlot = IsOverSlot(releasePos);

        // 判定B: 他の障害物、ボール、または「他のアイテム」と接触しているか？
        bool isOverlapping = overlapCount > 0;

        if (isOnSlot || isOverlapping)
        {
            // 条件に引っかかったら、枠の中（初期位置）へ戻る
            ReturnToSlot();
        }
        else
        {
            // 配置成功！元のサイズで確定
            transform.localScale = placedScale;
        }
    }

    // 指定した座標が枠（Slotタグ）の上にあるかを調べる関数
    private bool IsOverSlot(Vector2 position)
    {
        RaycastHit2D hitSlot = Physics2D.Raycast(position, Vector2.zero);
        return hitSlot.collider != null && hitSlot.collider.CompareTag("Slot");
    }

    // 枠に戻る処理
    public void ReturnToSlot()
    {
        transform.position = startPosition;
        transform.localScale = slotScale; // 枠サイズに戻す
        overlapCount = 0; // 接触カウントをリセット
    }

    // --- 接触判定（アイテム同士や壁との重なり検知） ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 枠(Slot)以外のオブジェクト（壁、ボール、他のアイテム）とぶつかったらカウント
        if (!collision.CompareTag("Slot"))
        {
            overlapCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Slot"))
        {
            overlapCount = Mathf.Max(0, overlapCount - 1);
        }
    }
}