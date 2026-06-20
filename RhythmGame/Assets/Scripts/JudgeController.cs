using UnityEngine;
using System.Collections.Generic;

public class JudgeController : MonoBehaviour
{
    public MusicSync musicSync;
    public NoteSpawner spawner;
    public Judge judge;

    public Transform[] judgeLines; // レーンごとの判定ライン
    public KeyCode[] laneKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };

    public float judgeRange = 0.35f; // ラインとの距離許容範囲

    void Update()
    {
        for (int lane = 0; lane < laneKeys.Length; lane++)
        {
            if (Input.GetKeyDown(laneKeys[lane]))
            {
                TryJudge(lane);
            }
        }
    }

    void TryJudge(int lane)
    {
        List<NoteData> notes = spawner.notes;

        NoteData target = null;
        Note noteObj = null;

        float bestDist = 999f;

        foreach (var n in notes)
        {
            if (n.lane != lane) continue;

            if (!spawner.noteObjects.ContainsKey(n)) continue;
            if (spawner.noteObjects[n] == null) continue;

            Note note = spawner.noteObjects[n];

            float dist = Mathf.Abs(note.transform.position.y - judgeLines[lane].position.y);

            if (dist < bestDist)
            {
                bestDist = dist;
                target = n;
                noteObj = note;
            }
        }

        if (target == null) return;

        if (bestDist > judgeRange)
        {
            judge.ShowMiss();
            return;
        }

        judge.JudgeNote(target);

        if (noteObj != null)
            Destroy(noteObj.gameObject);

        if (spawner.noteObjects.ContainsKey(target))
            spawner.noteObjects.Remove(target);

        notes.Remove(target);
    }
}
