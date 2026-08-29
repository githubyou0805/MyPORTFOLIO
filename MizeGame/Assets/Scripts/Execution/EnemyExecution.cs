using UnityEngine;

public class EnemyExecution : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn")]
    [SerializeField] private int minimumSpawnDistance = 5;
    [SerializeField] private int maximumSpawnDistance = 10;

    [Header("AI")]
    [SerializeField] private int detectionDistance = 7;
    [SerializeField] private int loseDistance = 12;

    [Header("Movement")]
    [SerializeField] private float moveInterval = 0.5f;

    private EnemyLogic logic;
    private GameObject enemyObject;

    private float moveTimer;

    private bool isChasing;

    /// <summary>
    /// プレイヤーから離れた位置に敵を生成
    /// </summary>
    public void SpawnNearPlayer()
    {
        logic = new EnemyLogic(
            System.Environment.TickCount,
            detectionDistance,
            loseDistance
        );

        if (!FindSpawnPosition(
            out int spawnX,
            out int spawnY))
        {
            Debug.LogWarning(
                "敵の出現位置が見つかりませんでした"
            );

            return;
        }

        logic.Spawn(
            spawnX,
            spawnY
        );

        Vector3 position =
            new Vector3(
                EnemyData.X,
                EnemyData.Y,
                -1
            );

        enemyObject = Instantiate(
            enemyPrefab,
            position,
            Quaternion.identity,
            transform
        );

        moveTimer = 0f;
        isChasing = false;

        Debug.Log(
            "敵が出現しました: " +
            EnemyData.X +
            ", " +
            EnemyData.Y
        );
    }

    /// <summary>
    /// 敵の行動
    /// </summary>
    private void Update()
    {
        if (logic == null ||
            !EnemyData.IsSpawned)
        {
            return;
        }

        moveTimer += Time.deltaTime;

        if (moveTimer < moveInterval)
        {
            return;
        }

        moveTimer = 0f;

        UpdateEnemyAI();
    }

    /// <summary>
    /// 敵AIを更新
    /// </summary>
    private void UpdateEnemyAI()
    {
        // まだ追跡していない
        if (!isChasing)
        {
            // プレイヤーを発見
            if (logic.CanSeePlayer())
            {
                isChasing = true;

                Debug.Log("敵がプレイヤーを発見！");
            }
            else
            {
                // ランダム巡回
                logic.Patrol();

                UpdateEnemyPosition();

                return;
            }
        }

        // 追跡中だがプレイヤーから離れた
        if (logic.HasLostPlayer())
        {
            isChasing = false;

            Debug.Log(
                "敵がプレイヤーを見失いました"
            );

            // 今回のターンでは移動しない
            return;
        }

        // プレイヤーを追跡
        logic.MoveTowardPlayer();

        UpdateEnemyPosition();

        // プレイヤーに追いついた
        if (EnemyData.X == PlayerData.X &&
            EnemyData.Y == PlayerData.Y)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 敵の出現位置を探す
    /// </summary>
    private bool FindSpawnPosition(
        out int spawnX,
        out int spawnY)
    {
        spawnX = 0;
        spawnY = 0;

        int maxAttempts = 2000;

        for (int i = 0; i < maxAttempts; i++)
        {
            int x =
                Random.Range(
                    1,
                    MazeData.Width - 1
                );

            int y =
                Random.Range(
                    1,
                    MazeData.Height - 1
                );

            // 壁なら不可
            if (MazeData.IsWall(x, y))
            {
                continue;
            }

            int distance =
                Mathf.Abs(
                    x - PlayerData.X
                )
                +
                Mathf.Abs(
                    y - PlayerData.Y
                );

            // 近すぎる場合
            if (distance < minimumSpawnDistance)
            {
                continue;
            }

            // 遠すぎる場合
            if (distance > maximumSpawnDistance)
            {
                continue;
            }

            // 扉の位置には置かない
            if (x == DoorData.X &&
                y == DoorData.Y)
            {
                continue;
            }

            spawnX = x;
            spawnY = y;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Unity上の敵位置を更新
    /// </summary>
    private void UpdateEnemyPosition()
    {
        if (enemyObject == null)
        {
            return;
        }

        enemyObject.transform.position =
            new Vector3(
                EnemyData.X,
                EnemyData.Y,
                -1
            );
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void GameOver()
    {
        Debug.Log("ゲームオーバー！");

        GameExecution.RestartScene();
    }
}