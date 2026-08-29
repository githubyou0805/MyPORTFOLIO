using UnityEngine;

public class MazeExecution : MonoBehaviour
{
    [Header("Maze")]
    [SerializeField] private int width = 21;
    [SerializeField] private int height = 21;

    [Header("Display")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;

    [Header("Random")]
    [SerializeField] private int seed = 0;

    private MazeGenerator generator;

    /// <summary>
    /// 最初の迷路を生成
    /// </summary>
    public void GenerateMaze()
    {
        InitializeGenerator();

        generator.Generate(
            width,
            height
        );

        CreateMazeObjects();

        Debug.Log("最初の迷路を生成しました");
    }

    /// <summary>
    /// 鍵取得後に迷路を再生成
    public void RegenerateMaze()
    {
        Debug.Log("迷路を再生成します");

        // 古い迷路を削除
        ClearMazeObjects();

        // 新しいSeedを作る
        int newSeed = System.Environment.TickCount;

        generator = new MazeGenerator(newSeed);

        if (EnemyData.IsSpawned)
        {
            // プレイヤー・扉・敵の位置を維持
            generator.Generate(
                width,
                height,

                PlayerData.X,
                PlayerData.Y,

                DoorData.InsideX,
                DoorData.InsideY,

                EnemyData.X,
                EnemyData.Y
            );
        }
        else
        {
            // プレイヤー・扉だけ維持
            generator.Generate(
                width,
                height,

                PlayerData.X,
                PlayerData.Y,

                DoorData.InsideX,
                DoorData.InsideY,

                -1,
                -1
            );
        }

        CreateMazeObjects();

        Debug.Log("新しい迷路を生成しました");
    }
    /// <summary>
    /// Generatorを初期化
    /// </summary>
    private void InitializeGenerator()
    {
        int actualSeed = seed;

        if (actualSeed == 0)
        {
            actualSeed =
                System.Environment.TickCount;
        }

        generator =
            new MazeGenerator(actualSeed);
    }

    /// <summary>
    /// 迷路を画面に生成
    /// </summary>
    private void CreateMazeObjects()
    {
        for (int x = 0;
             x < MazeData.Width;
             x++)
        {
            for (int y = 0;
                 y < MazeData.Height;
                 y++)
            {
                GameObject prefab;

                if (MazeData.IsWall(x, y))
                {
                    prefab = wallPrefab;
                }
                else
                {
                    prefab = floorPrefab;
                }

                if (prefab == null)
                {
                    continue;
                }

                Vector3 position =
                    new Vector3(x, y, 0);

                Instantiate(
                    prefab,
                    position,
                    Quaternion.identity,
                    transform
                );
            }
        }
    }

    /// <summary>
    /// 古い迷路を削除
    /// </summary>
    private void ClearMazeObjects()
    {
        for (int i = transform.childCount - 1;
             i >= 0;
             i--)
        {
            Destroy(
                transform.GetChild(i).gameObject
            );
        }
    }
}
