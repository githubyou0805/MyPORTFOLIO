using System;

public class KeyLogic
{
    private readonly Random random;

    private readonly int minimumDistance;

    public KeyLogic(int seed, int minimumDistance)
    {
        random = new Random(seed);
        this.minimumDistance = minimumDistance;
    }

    /// <summary>
    /// プレイヤーから離れた通路に鍵を配置
    /// </summary>
    public void SpawnAwayFromPlayer()
    {
        int maxAttempts = 2000;

        for (int i = 0; i < maxAttempts; i++)
        {
            int x = random.Next(MazeData.Width);
            int y = random.Next(MazeData.Height);

            if (MazeData.IsWall(x, y))
            {
                continue;
            }

            int distance =
                Math.Abs(x - PlayerData.X) +
                Math.Abs(y - PlayerData.Y);

            if (distance < minimumDistance)
            {
                continue;
            }

            // 扉の位置には置かない
            if (x == DoorData.X && y == DoorData.Y)
            {
                continue;
            }

            KeyData.SetPosition(x, y);
            return;
        }

        // 条件に合う場所が見つからなかった場合
        SpawnFarthestPosition();
    }

    /// <summary>
    /// プレイヤーから最も遠い通路に配置する
    /// </summary>
    private void SpawnFarthestPosition()
    {
        int bestX = 1;
        int bestY = 1;
        int bestDistance = -1;

        for (int x = 0; x < MazeData.Width; x++)
        {
            for (int y = 0; y < MazeData.Height; y++)
            {
                if (MazeData.IsWall(x, y))
                {
                    continue;
                }

                int distance =
                    Math.Abs(x - PlayerData.X) +
                    Math.Abs(y - PlayerData.Y);

                if (distance > bestDistance)
                {
                    bestDistance = distance;
                    bestX = x;
                    bestY = y;
                }
            }
        }

        KeyData.SetPosition(bestX, bestY);
    }

    /// <summary>
    /// プレイヤーが鍵を取得したか確認
    /// </summary>
    public bool TryCollect()
    {
        if (KeyData.IsCollected)
        {
            return false;
        }

        if (PlayerData.X != KeyData.X ||
            PlayerData.Y != KeyData.Y)
        {
            return false;
        }

        KeyData.Collect();

        return true;
    }
}
