using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class BossBulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform player;

    public float bulletSpeed = 3f;

    void SpawnBullet(Vector2 dir)
    {
        GameObject b = Instantiate(bulletPrefab, new Vector2(gameObject.transform.position.x,gameObject.transform.position.y - 2), Quaternion.identity);
        b.GetComponent<Rigidbody2D>().linearVelocity = dir.normalized * bulletSpeed;
    }

    Vector2 DirFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    // ============================
    // フェーズ1：ゆっくりリング弾
    // ============================
    public IEnumerator Phase1Pattern()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int a = 0; a < 36; a++)
            {
                SpawnBullet(DirFromAngle(a * 10f));
            }
            yield return new WaitForSeconds(1.2f);
        }
    }

    // ============================
    // フェーズ2：スパイラル弾
    // ============================
    public IEnumerator Phase2Pattern()
    {
        float angle = 0f;

        for (int i = 0; i < 80; i++)
        {
            SpawnBullet(DirFromAngle(angle));
            angle += 8f;
            yield return new WaitForSeconds(0.08f);
        }
    }

    // ============================
    // フェーズ3：狙い撃ち連射＋全方位
    // ============================
    public IEnumerator Phase3Pattern()
    {
        for (int i = 0; i < 10; i++)
        {
            // 狙い撃ち
            Vector2 dir = (player.position - transform.position).normalized;
            SpawnBullet(dir);

            // 全方位
            for (int a = 0; a < 12; a++)
            {
                SpawnBullet(DirFromAngle(a * 30f));
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
