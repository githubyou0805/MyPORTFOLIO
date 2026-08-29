using System;

public static class MazeData
{
    // 迷路の幅・高さ
    public static int Width { get; private set; }
    public static int Height { get; private set; }

    // true  = 壁
    // false = 通路
    private static bool[,] cells;

    /// <summary>
    /// 迷路データを初期化する
    /// </summary>
    public static void Initialize(int width, int height)
    {
        Width = width;
        Height = height;

        cells = new bool[width, height];
    }

    /// <summary>
    /// 指定座標が壁かどうか
    /// </summary>
    public static bool IsWall(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return true;
        }

        return cells[x, y];
    }

    /// <summary>
    /// 指定座標に壁を設定
    /// </summary>
    public static void SetWall(int x, int y, bool isWall)
    {
        if (!IsInside(x, y))
        {
            return;
        }

        cells[x, y] = isWall;
    }

    /// <summary>
    /// 指定座標が迷路内か
    /// </summary>
    public static bool IsInside(int x, int y)
    {
        return x >= 0 &&
               x < Width &&
               y >= 0 &&
               y < Height;
    }

    /// <summary>
    /// 迷路データを取得
    /// </summary>
    public static bool[,] GetCells()
    {
        return cells;
    }
}
