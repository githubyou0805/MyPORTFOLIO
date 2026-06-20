using UnityEngine;

public class Judge : MonoBehaviour
{
    public MusicSync musicSync;
    public JudgeUI judgeUI;

    public float perfectRange = 0.03f;
    public float goodRange = 0.08f;

    private void Start()
    {
        musicSync = GameObject.Find("GameManager").GetComponent<MusicSync>();
    }
    public void JudgeNote(NoteData note)
    {
        float diff = Mathf.Abs((float)musicSync.MusicTime - note.time);

        if (diff <= perfectRange)
        {
            judgeUI.Show("Perfect", Color.yellow);
        }
        else if (diff <= goodRange)
        {
            judgeUI.Show("Good", Color.green);
        }
        else
        {
            judgeUI.Show("Miss", Color.red);
        }
    }

    public void ShowMiss()
    {
        judgeUI.Show("Miss", Color.red);
    }
}
