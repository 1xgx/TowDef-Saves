using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private static readonly int[,] Directions = new int[,]
    {
        { 0, 1 },   // Right
        { 1, 0 },   // Bottom
        { 0, -1 },  // Left
        { -1, 0 }   // Top
    };

    private class Node
    {
        public int X { get; }
        public int Y { get; }
        public int G { get; set; }  // Cost of travelling from the start
        public int H { get; set; }  // Estimating the distance to the target
        public int F => G + H;      // Total cost

        public Node Parent { get; set; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
            G = 0;
            H = 0;
            Parent = null;
        }
    }

    private static int Heuristic(int x1, int y1, int x2, int y2)
    {
        // Manhattan Distance
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    private static bool IsWithinBounds(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private static bool IsWalkable(int[,] grid, int x, int y)
    {
        // Check that the cell is passable (e.g. 0 = passable, 1 = obstacle).
        return grid[x, y] == 0;
    }

    public static List<(int, int)> FindPath(int[,] grid, (int, int) start, (int, int) goal)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        var openSet = new List<Node>();
        var closedSet = new HashSet<(int, int)>();

        Node startNode = new Node(start.Item1, start.Item2);
        Node goalNode = new Node(goal.Item1, goal.Item2);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Find the node with the lowest cost F
            openSet.Sort((a, b) => a.F.CompareTo(b.F));
            Node current = openSet[0];

            if (current.X == goalNode.X && current.Y == goalNode.Y)
            {
                return ReconstructPath(current); // We found a way!
            }

            openSet.Remove(current);
            closedSet.Add((current.X, current.Y));

            // Check the neighbours
            for (int i = 0; i < 4; i++)
            {
                int neighborX = current.X + Directions[i, 0];
                int neighborY = current.Y + Directions[i, 1];

                if (!IsWithinBounds(neighborX, neighborY, width, height) || !IsWalkable(grid, neighborX, neighborY))
                {
                    continue; // Skip if the neighbour is off the map or unpassable
                }

                if (closedSet.Contains((neighborX, neighborY)))
                {
                    continue; // Skip if already processed
                }

                Node neighbor = new Node(neighborX, neighborY)
                {
                    G = current.G + 1, // Cost of the journey from the start to the neighbour
                    H = Heuristic(neighborX, neighborY, goalNode.X, goalNode.Y),
                    Parent = current
                };

                if (!openSet.Exists(n => n.X == neighborX && n.Y == neighborY && n.F <= neighbor.F))
                {
                    openSet.Add(neighbor);
                }
            }
        }

        return null; // Path not found
    }

    private static List<(int, int)> ReconstructPath(Node current)
    {
        var path = new List<(int, int)>();
        while (current != null)
        {
            path.Add((current.X, current.Y));
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }
}
