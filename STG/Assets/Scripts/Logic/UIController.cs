using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UIController : MonoBehaviour
{
    public UIData data;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ResultScore;
    public Slider hpSlider;
    public GameObject gameOverPanel;
    private Coroutine hpRoutine;
    private int score = 0;
    private float smoothHP;

    void Start()
    {
        UpdateScore(0);
        gameOverPanel.SetActive(false);
    }
    public void InitHP(int maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = maxHP;
        smoothHP = maxHP;
    }
    public void UpdateScore(int add)
    {
        score += add;
        scoreText.text = data.scorePrefix + score;
    }
    public void UpdateHP(int targetHP)
    {
        if (hpRoutine != null)
        {
            StopCoroutine(hpRoutine);
        }

        hpRoutine = StartCoroutine(SmoothHPChange(targetHP));
    }
    private System.Collections.IEnumerator SmoothHPChange(int targetHP)
    {
        while (Mathf.Abs(smoothHP - targetHP) > 0.01f)
        {
            smoothHP = Mathf.Lerp(smoothHP, targetHP, Time.deltaTime * 8f);
            hpSlider.value = smoothHP;
            yield return null;
        }

        smoothHP = targetHP;
        hpSlider.value = targetHP;
        hpRoutine = null;
    }
    public int GetCurrentScore()
    {
        return score;
    }
    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        ResultScore.text = data.scorePrefix + score.ToString();
    }
}
