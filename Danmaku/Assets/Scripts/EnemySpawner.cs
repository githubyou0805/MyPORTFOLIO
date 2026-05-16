using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    // 出現範囲（中心とサイズ）
    public Vector2 areaCenter = Vector2.zero;
    public Vector2 areaSize = new Vector2(10f, 5f);

    // 敵同士の最低距離
    public float minDistance = 2f;

    // すでに出現した敵の位置を記録
    private List<Vector2> spawnedPositions = new List<Vector2>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemy();

            // 出現間隔をランダムに（1.5〜3.5秒）
            float wait = Random.Range(1.5f, 3.5f);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnEnemy()
    {
        Vector2 pos;

        // 最大20回までランダム位置を探す
        for (int i = 0; i < 20; i++)
        {
            pos = GetRandomPosition();

            if (IsFarEnough(pos))
            {
                Instantiate(enemyPrefab, pos, Quaternion.identity);
                spawnedPositions.Add(pos);
                return;
            }
        }

        // 20回試しても見つからなければ諦める
        Debug.Log("敵の出現位置が見つかりませんでした");
    }

    // ランダム位置を取得
    Vector2 GetRandomPosition()
    {
        float x = Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2);
        float y = Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2);
        return new Vector2(x, y);
    }

    // 既存の敵と十分離れているかチェック
    bool IsFarEnough(Vector2 pos)
    {
        foreach (var p in spawnedPositions)
        {
            if (Vector2.Distance(p, pos) < minDistance)
                return false;
        }
        return true;
    }
}
