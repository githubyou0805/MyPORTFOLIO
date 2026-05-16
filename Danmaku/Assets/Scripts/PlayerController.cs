using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float invincibleTime = 1.5f; // 無敵時間
    public float blinkInterval = 0.1f;  // 点滅間隔

    private bool isInvincible = false;
    private SpriteRenderer sr;
    [SerializeField] private int HP = 3;
    private HPData data;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HP--;
        Destroy(collision.gameObject);
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
        // 敵弾に当たったら
        if (collision.CompareTag("EnemyBullet"))
        {
            if (!isInvincible)
            {
                StartCoroutine(InvincibleRoutine());
            }
        }
    }

    IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < invincibleTime)
        {
            // 点滅（表示/非表示）
            sr.enabled = !sr.enabled;

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // 最後に表示を戻す
        sr.enabled = true;

        isInvincible = false;
    }
}
