using UnityEngine;
using TMPro;

public class JudgeUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float fadeTime = 0.3f;
    public float showTime = 0.2f;

    private float timer = 0;
    private bool showing = false;

    void Start()
    {
        SetAlpha(0);
    }

    void Update()
    {
        if (!showing) return;

        timer += Time.deltaTime;

        if (timer < fadeTime)
        {
            // フェードイン
            SetAlpha(timer / fadeTime);
        }
        else if (timer < fadeTime + showTime)
        {
            // 表示維持
            SetAlpha(1);
        }
        else if (timer < fadeTime * 2 + showTime)
        {
            // フェードアウト
            float t = 1 - ((timer - fadeTime - showTime) / fadeTime);
            SetAlpha(t);
        }
        else
        {
            // 完全に消える
            SetAlpha(0);
            showing = false;
        }
    }

    public void Show(string message, Color color)
    {
        text.text = message;
        text.color = color;

        timer = 0;
        showing = true;
    }

    void SetAlpha(float a)
    {
        Color c = text.color;
        c.a = a;
        text.color = c;
    }
}
