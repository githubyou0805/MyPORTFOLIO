public static class DoorData
{
    public static int X { get; private set; }
    public static int Y { get; private set; }

    // 扉の1マス内側
    public static int InsideX { get; private set; }
    public static int InsideY { get; private set; }

    public static bool IsOpen { get; private set; }

    public static void SetPosition(
        int x,
        int y,
        int insideX,
        int insideY)
    {
        X = x;
        Y = y;

        InsideX = insideX;
        InsideY = insideY;

        IsOpen = false;
    }

    public static void Open()
    {
        IsOpen = true;
    }
}
