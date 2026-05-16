using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Vector2 moveDirection = Vector2.down;

    public BulletSpawner spawner;

    void Start()
    {
        // 弾幕パターン開始
        StartCoroutine(spawner.StartPattern());
    }

    void Update()
    {
        // 敵の移動
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
