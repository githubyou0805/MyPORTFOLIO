using UnityEngine;
using UnityEngine.SceneManagement;

public class GameExecution : MonoBehaviour
{
    [Header("Executions")]
    [SerializeField] private MazeExecution maze;
    [SerializeField] private PlayerExecution player;
    [SerializeField] private KeyExecution key;
    [SerializeField] private DoorExecution door;
    [SerializeField] private EnemyExecution enemy;

    private void Start()
    {
        StartGame();
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    private void StartGame()
    {
        Debug.Log("ゲーム開始");

        // 鍵取得イベントを登録
        key.OnKeyCollected += OnKeyCollected;

        // ① 迷路生成
        maze.GenerateMaze();

        // ② プレイヤー生成
        player.Initialize();
        player.Spawn();

        // ③ 扉生成
        door.Spawn();

        // ④ 鍵生成
        key.Spawn();

        // ⑤ 敵生成
        enemy.SpawnNearPlayer();
    }

    /// <summary>
    /// 鍵を取得した
    /// </summary>
    private void OnKeyCollected()
    {
        Debug.Log(
            "GameExecution：鍵取得を検知しました"
        );

        // 迷路を再生成
        maze.RegenerateMaze();

        // 扉も再生成
        door.Respawn();
        enemy.SpawnNearPlayer();

        Debug.Log(
            "迷路と扉を再生成しました"
        );
    }

    /// <summary>
    /// 現在のシーンを再読み込み
    /// </summary>
    public static void RestartScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    private void OnDestroy()
    {
        if (key != null)
        {
            key.OnKeyCollected -= OnKeyCollected;
        }
    }
}