using UnityEditor;
using UnityEngine;
using System.IO;

public class ChartEditorWindow : EditorWindow
{
    private AudioClip music;
    private AudioSource audioSource;

    private ChartData chart = new ChartData();
    private Vector2 scrollPos;

    private int selectedLane = 0;
    private bool isLongNote = false;
    private float longNoteEnd = 0;

    [MenuItem("Tools/Chart Editor")]
    public static void Open()
    {
        GetWindow<ChartEditorWindow>("Chart Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("複数レーン＋ロングノーツ対応 譜面エディタ", EditorStyles.boldLabel);

        music = (AudioClip)EditorGUILayout.ObjectField("Music", music, typeof(AudioClip), false);

        if (GUILayout.Button("再生"))
            PlayMusic();

        if (GUILayout.Button("停止"))
            StopMusic();

        GUILayout.Space(10);

        selectedLane = EditorGUILayout.IntSlider("レーン", selectedLane, 0, 3);
        isLongNote = EditorGUILayout.Toggle("ロングノーツ", isLongNote);

        if (isLongNote)
            longNoteEnd = EditorGUILayout.FloatField("ロングノーツ終了時間", longNoteEnd);

        if (GUILayout.Button("現在の再生位置にノーツ追加"))
            AddNote();

        GUILayout.Space(10);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        foreach (var note in chart.notes)
        {
            EditorGUILayout.BeginHorizontal();

            note.time = EditorGUILayout.FloatField("Start", note.time);
            note.endTime = EditorGUILayout.FloatField("End", note.endTime);
            note.lane = EditorGUILayout.IntField("Lane", note.lane);
            note.type = EditorGUILayout.TextField("Type", note.type);

            if (GUILayout.Button("削除", GUILayout.Width(60)))
            {
                chart.notes.Remove(note);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("保存"))
            SaveChart();

        if (GUILayout.Button("読み込み"))
            LoadChart();
    }

    void PlayMusic()
    {
        if (music == null) return;

        if (audioSource == null)
        {
            GameObject obj = new GameObject("EditorAudio");
            audioSource = obj.AddComponent<AudioSource>();
        }

        audioSource.clip = music;
        audioSource.Play();
    }

    void StopMusic()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    void AddNote()
    {
        float t = audioSource != null ? audioSource.time : 0;

        NoteData note = new NoteData
        {
            time = t,
            lane = selectedLane,
            type = isLongNote ? "long" : "tap",
            endTime = isLongNote ? longNoteEnd : 0
        };

        chart.notes.Add(note);
        Debug.Log("ノーツ追加: lane=" + selectedLane + " time=" + t);
    }

    void SaveChart()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "chart.json");
        string json = JsonUtility.ToJson(chart, true);
        File.WriteAllText(path, json);
        Debug.Log("保存しました: " + path);
    }
    void LoadChart()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "chart.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("chart.json がありません: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        chart = JsonUtility.FromJson<ChartData>(json);
        Debug.Log("読み込み完了");
    }
}
