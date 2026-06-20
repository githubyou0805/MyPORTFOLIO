using System;
using System.Collections.Generic;

[Serializable]
public class NoteData
{
    public float time;      // 開始時間
    public float endTime;   // ロングノーツ用（tap の場合は 0）
    public int lane;        // レーン番号
    public string type;     // "tap" or "long"
}

[Serializable]
public class ChartData
{
    public float bpm = 120;
    public float offset = 0;
    public List<NoteData> notes = new List<NoteData>();
}
