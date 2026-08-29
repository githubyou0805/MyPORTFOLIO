public static class KeyData
{
    public static int X { get; private set; }
    public static int Y { get; private set; }

    public static bool IsCollected { get; private set; }

    public static void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
        IsCollected = false;
    }

    public static void Collect()
    {
        IsCollected = true;
    }
}
