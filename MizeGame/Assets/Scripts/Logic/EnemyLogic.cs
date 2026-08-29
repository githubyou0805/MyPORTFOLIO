using System;
using System.Collections.Generic;

public class EnemyLogic
{
    private readonly Random random;

    private readonly int detectionDistance;
    private readonly int loseDistance;

    public EnemyLogic(
        int seed,
        int detectionDistance,
        int loseDistance)
    {
        random = new Random(seed);

        this.detectionDistance = detectionDistance;
        this.loseDistance = loseDistance;
    }

    /// <summary>
    /// 敵を指定位置に生成
    /// </summary>
    public void Spawn(int x, int y)
    {
        EnemyData.SetPosition(x, y);
    }

    /// <summary>
    /// 迷路の通路を使ったプレイヤーまでの最短距離を取得
    /// </summary>
    private int GetDistanceToPlayer()
    {
        Queue<Node> queue = new Queue<Node>();

        bool[,] visited =
            new bool[
                MazeData.Width,
                MazeData.Height
            ];

        Node start = new Node(
            EnemyData.X,
            EnemyData.Y,
            null,
            0
        );

        queue.Enqueue(start);

        visited[
            EnemyData.X,
            EnemyData.Y
        ] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            // プレイヤーに到達
            if (current.X == PlayerData.X &&
                current.Y == PlayerData.Y)
            {
                return current.Distance;
            }

            for (int i = 0; i < 4; i++)
            {
                int nextX = current.X + dx[i];
                int nextY = current.Y + dy[i];

                if (!MazeData.IsInside(nextX, nextY))
                {
                    continue;
                }

                if (visited[nextX, nextY])
                {
                    continue;
                }

                // 壁は通れない
                if (MazeData.IsWall(nextX, nextY))
                {
                    continue;
                }

                visited[nextX, nextY] = true;

                Node next = new Node(
                    nextX,
                    nextY,
                    current,
                    current.Distance + 1
                );

                queue.Enqueue(next);
            }
        }

        // プレイヤーまでの道が存在しない
        return int.MaxValue;
    }

    /// <summary>
    /// プレイヤーを発見しているか
    /// </summary>
    public bool CanSeePlayer()
    {
        int distance = GetDistanceToPlayer();

        return distance <= detectionDistance;
    }

    /// <summary>
    /// プレイヤーを見失ったか
    /// </summary>
    public bool HasLostPlayer()
    {
        int distance = GetDistanceToPlayer();

        return distance > loseDistance;
    }

    /// <summary>
    /// プレイヤーへ最短経路で1マス移動する
    /// </summary>
    public bool MoveTowardPlayer()
    {
        Node nextStep = FindNextStepToPlayer();

        if (nextStep == null)
        {
            return false;
        }

        EnemyData.SetPosition(
            nextStep.X,
            nextStep.Y
        );

        return true;
    }

    /// <summary>
    /// ランダムに1マス巡回する
    /// </summary>
    public bool Patrol()
    {
        List<Node> candidates =
            new List<Node>();

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int nextX = EnemyData.X + dx[i];
            int nextY = EnemyData.Y + dy[i];

            if (!MazeData.IsInside(nextX, nextY))
            {
                continue;
            }

            if (MazeData.IsWall(nextX, nextY))
            {
                continue;
            }

            candidates.Add(
                new Node(
                    nextX,
                    nextY,
                    null,
                    0
                )
            );
        }

        if (candidates.Count == 0)
        {
            return false;
        }

        int index =
            random.Next(candidates.Count);

        Node selected =
            candidates[index];

        EnemyData.SetPosition(
            selected.X,
            selected.Y
        );

        return true;
    }

    /// <summary>
    /// プレイヤーまでの最短経路を探す
    /// </summary>
    private Node FindNextStepToPlayer()
    {
        Queue<Node> queue =
            new Queue<Node>();

        bool[,] visited =
            new bool[
                MazeData.Width,
                MazeData.Height
            ];

        Node start = new Node(
            EnemyData.X,
            EnemyData.Y,
            null,
            0
        );

        queue.Enqueue(start);

        visited[
            EnemyData.X,
            EnemyData.Y
        ] = true;

        Node goal = null;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            Node current =
                queue.Dequeue();

            if (current.X == PlayerData.X &&
                current.Y == PlayerData.Y)
            {
                goal = current;
                break;
            }

            for (int i = 0; i < 4; i++)
            {
                int nextX =
                    current.X + dx[i];

                int nextY =
                    current.Y + dy[i];

                if (!MazeData.IsInside(
                    nextX,
                    nextY))
                {
                    continue;
                }

                if (visited[nextX, nextY])
                {
                    continue;
                }

                if (MazeData.IsWall(
                    nextX,
                    nextY))
                {
                    continue;
                }

                visited[nextX, nextY] = true;

                Node next = new Node(
                    nextX,
                    nextY,
                    current,
                    current.Distance + 1
                );

                queue.Enqueue(next);
            }
        }

        if (goal == null)
        {
            return null;
        }

        // ゴールから逆にたどり、
        // 敵の次の1歩を取得
        Node step = goal;

        while (
            step.Parent != null &&
            step.Parent.Parent != null)
        {
            step = step.Parent;
        }

        return step;
    }

    private class Node
    {
        public int X;
        public int Y;
        public Node Parent;
        public int Distance;

        public Node(
            int x,
            int y,
            Node parent,
            int distance)
        {
            X = x;
            Y = y;
            Parent = parent;
            Distance = distance;
        }
    }
}