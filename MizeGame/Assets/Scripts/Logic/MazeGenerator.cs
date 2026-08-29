using System;
using System.Collections.Generic;

public class MazeGenerator
{
    private readonly Random random;

    public MazeGenerator(int seed)
    {
        random = new Random(seed);
    }

    /// <summary>
    /// 通常の迷路生成
    /// </summary>
    public void Generate(int width, int height)
    {
        Generate(
            width,
            height,
            -1,
            -1,
            -1,
            -1,
            -1,
            -1
        );
    }
    /// <summary>
    /// 迷路に複数のルートを作る
    /// 通路幅は必ず1マスにする
    /// </summary>
    private void CreateLoops()
    {
        int loopCount = 30;

        for (int i = 0; i < loopCount; i++)
        {
            int x = random.Next(2, MazeData.Width - 2);
            int y = random.Next(2, MazeData.Height - 2);

            // すでに通路なら対象外
            if (!MazeData.IsWall(x, y))
            {
                continue;
            }

            bool horizontal =
                !MazeData.IsWall(x - 1, y) &&
                !MazeData.IsWall(x + 1, y);

            bool vertical =
                !MazeData.IsWall(x, y - 1) &&
                !MazeData.IsWall(x, y + 1);

            // 左右をつなぐ場合
            if (horizontal && !vertical)
            {
                // 上下が両方とも壁であることを確認
                if (MazeData.IsWall(x, y - 1) &&
                    MazeData.IsWall(x, y + 1))
                {
                    MazeData.SetWall(x, y, false);
                }
            }

            // 上下をつなぐ場合
            else if (vertical && !horizontal)
            {
                // 左右が両方とも壁であることを確認
                if (MazeData.IsWall(x - 1, y) &&
                    MazeData.IsWall(x + 1, y))
                {
                    MazeData.SetWall(x, y, false);
                }
            }
        }
    }
         /// <summary>
         /// 指定した2地点を必ず通路にする迷路生成
         /// </summary>
    public void Generate(
        int width,
        int height,
        int requiredX1,
        int requiredY1,
        int requiredX2,
        int requiredY2,
        int requiredX3,
        int requiredY3)
    {
        int maxAttempts = 1000;

        for (int attempt = 0;
             attempt < maxAttempts;
             attempt++)
        {
            MazeData.Initialize(width, height);

            FillWalls();

            Dig(1, 1);

            // 複数のルートを作る
            CreateLoops();

            // 必ず通路にする地点
            bool point1Valid =
                requiredX1 < 0 ||
                !MazeData.IsWall(
                    requiredX1,
                    requiredY1
                );

            bool point2Valid =
                requiredX2 < 0 ||
                !MazeData.IsWall(
                    requiredX2,
                    requiredY2
                );

            bool point3Valid =
                requiredX3 < 0 ||
                !MazeData.IsWall(
                    requiredX3,
                    requiredY3
                );

            if (point1Valid &&
                point2Valid &&
                point3Valid)
            {
                return;
            }

            if (point1Valid && point2Valid)
            {
                return;
            }
        }

        // 万が一1000回失敗した場合
        MazeData.Initialize(width, height);
        FillWalls();
        Dig(1, 1);

        // 最低限、指定地点を通路にする
        if (requiredX1 >= 0)
        {
            MazeData.SetWall(
                requiredX1,
                requiredY1,
                false
            );
        }

        if (requiredX2 >= 0)
        {
            MazeData.SetWall(
                requiredX2,
                requiredY2,
                false
            );
        }
    }

    private void FillWalls()
    {
        for (int x = 0; x < MazeData.Width; x++)
        {
            for (int y = 0; y < MazeData.Height; y++)
            {
                MazeData.SetWall(x, y, true);
            }
        }
    }

    private void Dig(int x, int y)
    {
        MazeData.SetWall(x, y, false);

        List<Direction> directions =
            CreateShuffledDirections();

        foreach (Direction direction in directions)
        {
            int nextX = x + direction.X * 2;
            int nextY = y + direction.Y * 2;

            if (!MazeData.IsInside(nextX, nextY))
            {
                continue;
            }

            if (!MazeData.IsWall(nextX, nextY))
            {
                continue;
            }

            int wallX = x + direction.X;
            int wallY = y + direction.Y;

            MazeData.SetWall(
                wallX,
                wallY,
                false
            );

            Dig(nextX, nextY);
        }
    }

    private List<Direction> CreateShuffledDirections()
    {
        List<Direction> directions =
            new List<Direction>
            {
                new Direction(1, 0),
                new Direction(-1, 0),
                new Direction(0, 1),
                new Direction(0, -1)
            };

        for (int i = directions.Count - 1;
             i > 0;
             i--)
        {
            int j = random.Next(i + 1);

            Direction temp = directions[i];
            directions[i] = directions[j];
            directions[j] = temp;
        }

        return directions;
    }

    private struct Direction
    {
        public int X;
        public int Y;

        public Direction(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
