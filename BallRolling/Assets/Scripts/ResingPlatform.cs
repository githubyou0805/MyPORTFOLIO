using UnityEngine;
using System.Collections;

public class RisingPlatform : MonoBehaviour
{
    [Header("上昇する高さ")]
    [SerializeField] private float riseHeight = 5f;

    [Header("移動スピード")]
    [SerializeField] private float speed = 3f;

    [Header("プレイヤーが離れてから戻り始めるまでの遅延（秒）")]
    [SerializeField] private float returnDelay = 0.5f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Rigidbody rb;
    private bool isPlayerOn = false;
    private Coroutine returnCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 物理的にプレイヤーを押し上げるため、Rigidbodyの設定を最適化します
        if (rb != null)
        {
            rb.isKinematic = true;  // スクリプトから物理的に安全に動かす設定
            rb.useGravity = false; // 重力で勝手に落ちないようにする
        }
        // 初期位置と目標位置をセット
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * riseHeight;
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // プレイヤーが乗っているなら目標は「上昇先」、離れたなら「初期位置」
        Vector3 destination = isPlayerOn ? targetPosition : startPosition;

        // 現在地から目的地まで物理的にスムーズに移動
        Vector3 nextPosition = Vector3.MoveTowards(rb.position, destination, speed * Time.fixedDeltaTime);
        rb.MovePosition(nextPosition);
    }

    // プレイヤーが上に乗ったときの判定
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ボールが「床の上側」に触れたときだけ上昇させる判定（横や下からの接触を無視）
            if (collision.contacts[0].normal.y < -0.5f)
            {
                // もし戻り中（ディレイ中）のコルーチンが動いていたら止める
                if (returnCoroutine != null)
                {
                    StopCoroutine(returnCoroutine);
                    returnCoroutine = null;
                }
                isPlayerOn = true;
            }
        }
    }

    // プレイヤーが離れたときの判定
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ボールがジャンプしたり一瞬跳ねたりしただけで床が急降下するのを防ぐため、
            // 指定された秒数（returnDelay）だけ待ってから戻る処理を呼び出します
            if (returnCoroutine != null) StopCoroutine(returnCoroutine);
            returnCoroutine = StartCoroutine(StartReturnDelay());
        }
    }

    private IEnumerator StartReturnDelay()
    {
        yield return new WaitForSeconds(returnDelay);
        isPlayerOn = false;
        returnCoroutine = null;
    }
}