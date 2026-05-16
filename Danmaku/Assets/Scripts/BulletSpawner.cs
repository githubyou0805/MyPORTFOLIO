using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform player;

    public float bulletSpeed = 2.5f;

    void SpawnBullet(Vector2 direction)
    {
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        b.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;
    }

    Vector2 DirFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    // ============================
    // ランダムパターン → 終了後に敵を消す
    // ============================
    public IEnumerator StartPattern()
    {
        int r = Random.Range(0, 2);

        switch (r)
        {
            case 0:
                yield return StartCoroutine(SpiralShot());
                break;
            case 1:
                yield return StartCoroutine(RingShot());
                break;
        }

        // ★ パターン終了後に敵を消す
        Destroy(gameObject);
    }

    // ============================
    // 弾幕パターン
    // ============================

    IEnumerator SpiralShot()
    {
        float angle = 0f;

        for (int i = 0; i < 20; i++)
        {
            SpawnBullet(DirFromAngle(angle));
            angle += 12f;
            yield return new WaitForSeconds(0.12f);
        }
    }

    IEnumerator RingShot()
    {
        for (int i = 0; i < 24; i++)
        {
            float angle = i * (360f / 24);
            SpawnBullet(DirFromAngle(angle));
        }

        yield return new WaitForSeconds(1.2f);
    }
}
