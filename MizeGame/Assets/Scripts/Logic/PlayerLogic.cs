using System;

public class PlayerLogic
{
    private readonly Random random;

    public PlayerLogic(int seed)
    {
        random = new Random(seed);
    }

    /// <summary>
    /// 迷路内のランダムな通路にプレイヤーを配置する
    /// </summary>
    public void SpawnRandom()
    {
        int maxAttempts = 1000;

        for (int i = 0; i < maxAttempts; i++)
        {
            int x = random.Next(MazeData.Width);
            int y = random.Next(MazeData.Height);

            // 通路ならスポーン
            if (!MazeData.IsWall(x, y))
            {
                PlayerData.SetPosition(x, y);
                return;
            }
        }

        // ランダムで見つからなかった場合の保険
        SpawnFirstFloor();
    }

    /// <summary>
    /// 指定方向へ移動する
    /// </summary>
    public bool Move(int x, int y)
    {
        int nextX = PlayerData.X + x;
        int nextY = PlayerData.Y + y;

        // 開いた扉のマスなら移動可能
        bool isOpenDoor =
            DoorData.IsOpen &&
            nextX == DoorData.X &&
            nextY == DoorData.Y;

        // 壁なら移動しない
        if (MazeData.IsWall(nextX, nextY) && !isOpenDoor)
        {
            return false;
        }

        PlayerData.SetPosition(nextX, nextY);

        return true;
    }
    /// <summary>
    /// 開いた扉の位置にいるか
    /// </summary>
    public bool IsAtOpenDoor()
    {
        return DoorData.IsOpen &&
               PlayerData.X == DoorData.X &&
               PlayerData.Y == DoorData.Y;
    }
    /// <summary>
    /// 最初に見つかった通路に配置
    /// </summary>
    private void SpawnFirstFloor()
    {
        for (int x = 0; x < MazeData.Width; x++)
        {
            for (int y = 0; y < MazeData.Height; y++)
            {
                if (!MazeData.IsWall(x, y))
                {
                    PlayerData.SetPosition(x, y);
                    return;
                }
            }
        }
    }
}
