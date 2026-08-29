public static class EnemyData
{
    public static int X { get; private set; }
    public static int Y { get; private set; }

    public static bool IsSpawned { get; private set; }

    /// <summary>
    /// 敵の位置を設定
    /// </summary>
    public static void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
        IsSpawned = true;
    }

    /// <summary>
    /// 敵を未出現状態にする
    /// </summary>
    public static void Reset()
    {
        X = 0;
        Y = 0;
        IsSpawned = false;
    }
}