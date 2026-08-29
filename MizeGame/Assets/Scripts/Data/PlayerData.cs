public static class PlayerData
{
    public static int X { get; private set; }
    public static int Y { get; private set; }

    /// <summary>
    /// プレイヤーの位置を設定する
    /// </summary>
    public static void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }
}
