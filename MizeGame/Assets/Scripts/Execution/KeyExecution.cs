using UnityEngine;
using System;

public class KeyExecution : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;

    [SerializeField] private int minimumDistance = 10;

    private KeyLogic logic;
    private GameObject keyObject;

    public Action OnKeyCollected;

    /// <summary>
    /// 鍵を生成する
    /// </summary>
    public void Spawn()
    {
        int seed = Environment.TickCount;

        logic = new KeyLogic(
            seed,
            minimumDistance
        );

        logic.SpawnAwayFromPlayer();

        Vector3 position = new Vector3(
            KeyData.X,
            KeyData.Y,
            -1
        );

        keyObject = Instantiate(
            keyPrefab,
            position,
            Quaternion.identity,
            transform
        );
    }

    private void Update()
    {
        if (logic == null)
        {
            return;
        }

        if (logic.TryCollect())
        {
            CollectKey();
        }
    }

    /// <summary>
    /// 鍵を取得したときの処理
    /// </summary>
    private void CollectKey()
    {
        Debug.Log("鍵を取得しました！");

        if (keyObject != null)
        {
            Destroy(keyObject);
            keyObject = null;
        }

        OnKeyCollected?.Invoke();
    }
}
