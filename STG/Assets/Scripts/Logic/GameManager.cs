using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private UIController ui;
    private PlayerController player;

    public GameObject enemyPrefab;
    public float spawnInterval = 1.5f;
    private float timer = 0f;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ui = Object.FindFirstObjectByType<UIController>();
        player = Object.FindFirstObjectByType<PlayerController>();

        InitGame();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }
    public void InitGame()
    {
        // スコア初期化
        ui.UpdateScore(-ui.GetCurrentScore()); // スコアを0に戻す

        // HP初期化
        player.ResetStatus();
        ui.InitHP(player.maxHP);

        // ゲームオーバー画面非表示
        ui.HideGameOver();
    }
    void SpawnEnemy()
    {
        float x = Random.Range(-3f, 3f);
        Vector3 pos = new Vector3(x, 6f, 0);
        Instantiate(enemyPrefab, pos,Quaternion.Euler(0,0,180));
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
