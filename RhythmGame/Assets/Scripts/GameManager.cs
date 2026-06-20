using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MusicSync musicSync;

    void Update()
    {
        if (musicSync.MusicTime >= 60) // 曲が終わったら
        {
            SceneManager.LoadScene("Result");
        }
    }
}
