using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public MusicSync musicSync;

    public List<NoteData> notes = new List<NoteData>();
    private int index = 0;

    void Start()
    {
        LoadChart();

        if (notes == null)
        {
            Debug.LogError("譜面データが読み込めませんでした");
            notes = new List<NoteData>();
        }
    }

    void Update()
    {
        if (notes == null || notes.Count == 0) return;

        double current = musicSync.MusicTime;

        if (index < notes.Count && current >= notes[index].time - 1.0)
        {
            SpawnNote(notes[index]);
            index++;
        }
    }

    void LoadChart()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "chart.json");

        if (!File.Exists(path))
        {
            Debug.LogError("chart.json が見つかりません: " + path);
            notes = new List<NoteData>();
            return;
        }

        string json = File.ReadAllText(path);
        ChartData chart = JsonUtility.FromJson<ChartData>(json);

        if (chart == null)
        {
            Debug.LogError("chart.json の読み込みに失敗しました");
            notes = new List<NoteData>();
            return;
        }

        notes = chart.notes;
        Debug.Log("譜面読み込み成功: " + notes.Count + " ノーツ");
    }

    public Dictionary<NoteData, Note> noteObjects = new Dictionary<NoteData, Note>();

    void SpawnNote(NoteData data)
    {
        Vector3 pos = new Vector3(data.lane * 1.5f - 2.0f, 5, 0);
        GameObject obj = Instantiate(notePrefab, pos, Quaternion.identity);

        Note note = obj.GetComponent<Note>();
        note.data = data;

        noteObjects[data] = note;
    }
}
