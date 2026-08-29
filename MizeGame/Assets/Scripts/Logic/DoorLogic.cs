using System;
using System.Collections.Generic;

public class DoorLogic
{
    private readonly Random random;

    private readonly int minimumDistanceFromPlayer;

    public DoorLogic(int seed, int minimumDistanceFromPlayer)
    {
        random = new Random(seed);
        this.minimumDistanceFromPlayer = minimumDistanceFromPlayer;
    }

    /// <summary>
    /// プレイヤーから離れた外周に扉を配置
    /// </summary>
    public void SpawnAwayFromPlayer()
    {
        List<DoorPosition> candidates = CreateCandidates();

        Shuffle(candidates);

        foreach (DoorPosition candidate in candidates)
        {
            int distance =
                Math.Abs(candidate.InsideX - PlayerData.X) +
                Math.Abs(candidate.InsideY - PlayerData.Y);

            if (distance < minimumDistanceFromPlayer)
            {
                continue;
            }

            // 最初の迷路で扉の内側が通路になる場所を探す
            if (!MazeData.IsWall(
                    candidate.InsideX,
                    candidate.InsideY))
            {
                DoorData.SetPosition(
                    candidate.X,
                    candidate.Y,
                    candidate.InsideX,
                    candidate.InsideY
                );

                return;
            }
        }

        // 条件に合わなかった場合は最も遠い候補
        SpawnFarthestDoor(candidates);
    }

    /// <summary>
    /// 扉の前にいるか
    /// </summary>
    public bool IsPlayerAtDoor()
    {
        return PlayerData.X == DoorData.InsideX &&
               PlayerData.Y == DoorData.InsideY;
    }

    /// <summary>
    /// 鍵取得後、Enterで扉を開く
    /// </summary>
    public bool TryOpen()
    {
        if (KeyData.IsCollected == false)
        {
            return false;
        }

        if (DoorData.IsOpen)
        {
            return false;
        }

        if (!IsPlayerAtDoor())
        {
            return false;
        }

        DoorData.Open();

        return true;
    }

    private List<DoorPosition> CreateCandidates()
    {
        List<DoorPosition> candidates =
            new List<DoorPosition>();

        // 左
        for (int y = 1; y < MazeData.Height - 1; y += 2)
        {
            candidates.Add(
                new DoorPosition(
                    0,
                    y,
                    1,
                    y
                )
            );
        }

        // 右
        for (int y = 1; y < MazeData.Height - 1; y += 2)
        {
            candidates.Add(
                new DoorPosition(
                    MazeData.Width - 1,
                    y,
                    MazeData.Width - 2,
                    y
                )
            );
        }

        // 下
        for (int x = 1; x < MazeData.Width - 1; x += 2)
        {
            candidates.Add(
                new DoorPosition(
                    x,
                    0,
                    x,
                    1
                )
            );
        }

        // 上
        for (int x = 1; x < MazeData.Width - 1; x += 2)
        {
            candidates.Add(
                new DoorPosition(
                    x,
                    MazeData.Height - 1,
                    x,
                    MazeData.Height - 2
                )
            );
        }

        return candidates;
    }

    private void SpawnFarthestDoor(
        List<DoorPosition> candidates)
    {
        DoorPosition best = candidates[0];
        int bestDistance = -1;

        foreach (DoorPosition candidate in candidates)
        {
            int distance =
                Math.Abs(candidate.InsideX - PlayerData.X) +
                Math.Abs(candidate.InsideY - PlayerData.Y);

            if (distance > bestDistance)
            {
                bestDistance = distance;
                best = candidate;
            }
        }

        DoorData.SetPosition(
            best.X,
            best.Y,
            best.InsideX,
            best.InsideY
        );
    }

    private void Shuffle(List<DoorPosition> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);

            DoorPosition temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private struct DoorPosition
    {
        public int X;
        public int Y;

        public int InsideX;
        public int InsideY;

        public DoorPosition(
            int x,
            int y,
            int insideX,
            int insideY)
        {
            X = x;
            Y = y;
            InsideX = insideX;
            InsideY = insideY;
        }
    }
}
