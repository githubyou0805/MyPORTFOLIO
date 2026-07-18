using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("追従対象（ボール）")]
    [SerializeField] private Transform target;

    [Header("ボールとの基本距離（高さや奥行き）")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -6f);

    [Header("カメラが動き出す、ボールとの限界距離")]
    [SerializeField] private float returnThreshold = 3f;

    [Header("元の位置に戻るスピード")]
    [SerializeField] private float returnSpeed = 3f;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        // 初期位置を固定距離にセット
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 本来カメラがあるべき「元の位置（定位置）」を計算
        Vector3 defaultPosition = target.position + offset;

        // 現在のカメラの位置と、あるべき元の位置との距離を測る
        float distance = Vector3.Distance(transform.position, defaultPosition);

        // プレイヤーが離れて、設定した限界距離を超えたら元の位置に戻る
        if (distance > returnThreshold)
        {
            // Lerpを使ってスムーズに元の位置（defaultPosition）へ戻していく
            transform.position = Vector3.Lerp(transform.position, defaultPosition, returnSpeed * Time.deltaTime);
        }
    }
}