using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Rigidbody2D ballRigidbody;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;

    private Vector3 ballStartPosition;
    private bool isPlaying = false;

    public bool IsPlaying => isPlaying; // 他のスクリプトから状態を見る用

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // ボールの初期位置を記憶
        if (ballRigidbody != null)
        {
            ballStartPosition = ballRigidbody.transform.position;
            SetPhysicsActive(false); // 最初は物理を止める
        }

        // ボタンのイベント登録
        if (startButton != null) startButton.onClick.AddListener(StartSimulation);
        if (resetButton != null) resetButton.onClick.AddListener(ResetSimulation);

        if (resetButton != null) resetButton.interactable = false; // 最初はリセットを押せないように
    }

    // 「ドーン！」（再生開始）
    public void StartSimulation()
    {
        isPlaying = true;
        SetPhysicsActive(true);

        if (startButton != null) startButton.interactable = false;
        if (resetButton != null) resetButton.interactable = true;
    }

    // やり直し（配置フェーズに戻る）
    public void ResetSimulation()
    {
        isPlaying = false;
        SetPhysicsActive(false);

        // ボールを元の位置に戻す
        if (ballRigidbody != null)
        {
            ballRigidbody.transform.position = ballStartPosition;
            ballRigidbody.linearVelocity = Vector2.zero; // Unity 6推奨
            ballRigidbody.angularVelocity = 0f;
        }

        // ステージ上のすべてのアイテムを枠に戻す
        DraggableItem[] items = Object.FindObjectsByType<DraggableItem>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            item.ReturnToSlot();
        }

        if (startButton != null) startButton.interactable = true;
        if (resetButton != null) resetButton.interactable = false;
    }

    // エラーの原因だった関数です。ここにしっかり定義しておきます！
    private void SetPhysicsActive(bool active)
    {
        if (ballRigidbody == null) return;

        // Kinematic（物理影響なし）と Dynamic（物理影響あり）を切り替える
        ballRigidbody.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
    }
}