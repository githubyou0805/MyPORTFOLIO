using UnityEngine;

public class PlayerExecution : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Random")]
    [SerializeField] private int seed = 0;
    public System.Action<int, int> OnPlayerMoved;

    private PlayerLogic logic;
    private GameObject playerObject;
    [SerializeField] private CameraFollow cameraFollow;
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// プレイヤーのロジックを初期化する
    /// </summary>
    public void Initialize()
    {
        int actualSeed = seed;

        if (actualSeed == 0)
        {
            actualSeed = System.Environment.TickCount;
        }

        logic = new PlayerLogic(actualSeed);
    }

    public void Spawn()
    {
        if (logic == null)
        {
            Initialize();
        }

        logic.SpawnRandom();

        Vector3 position = new Vector3(
            PlayerData.X,
            PlayerData.Y,
            -1
        );

        playerObject = Instantiate(
            playerPrefab,
            position,
            Quaternion.identity
        );
        cameraFollow.SetTarget(playerObject.transform);
    }

    /// <summary>
    /// プレイヤーをランダムな通路に配置する
    /// </summary>
    private void SpawnPlayer()
    {
        logic.SpawnRandom();

        Vector3 position = new Vector3(
            PlayerData.X,
            PlayerData.Y,
            -1
        );

        playerObject = Instantiate(
            playerPrefab,
            position,
            Quaternion.identity
        );
        cameraFollow.SetTarget(playerObject.transform);
    }

    /// <summary>
    /// WASD入力を処理する
    /// </summary>
    private void HandleInput()
    {
        int moveX = 0;
        int moveY = 0;

        if (Input.GetKeyDown(KeyCode.W))
        {
            moveY = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            moveY = -1;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            moveX = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            moveX = 1;
        }

        // 移動入力がなければ何もしない
        if (moveX == 0 && moveY == 0)
        {
            return;
        }

        // Logicに移動を依頼
        bool moved = logic.Move(moveX, moveY);

        // 移動できた場合だけUnity上の位置を変更
        if (moved)
        {
            UpdatePlayerPosition();

            // プレイヤーが移動したことを通知
            OnPlayerMoved?.Invoke(
                PlayerData.X,
                PlayerData.Y
            );

            // 扉を通過したらクリア
            if (logic.IsAtOpenDoor())
            {
                GameClear();
            }
        }
    }
    /// <summary>
    /// 扉を通過したか確認
    /// </summary>
    private void GameClear()
    {
        Debug.Log("ゲームクリア！");

        // プレイヤーの操作を停止
        enabled = false;
    }
    /// <summary>
    /// Dataの位置をUnityのGameObjectに反映する
    /// </summary>
    private void UpdatePlayerPosition()
    {
        playerObject.transform.position = new Vector3(
            PlayerData.X,
            PlayerData.Y,
            -1
        );
    }
}
