using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MissleCellController : MonoBehaviour
{
    [SerializeField] private Transform[] _allHexagons;
    [SerializeField] private Transform[,] _allHexagons2DArray;
    private List<Transform> _path;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Transform _target;

    [SerializeField] private float _damage = 3.0f;

    [SerializeField] private GridCell _gridCell;
    private int[,] _cellNumbers;
    private GameObject[,] _cell;
    private List<(int, int)> _missleWay;
    public (int, int) _MissleCellPosition;
    private (int, int) _goal;
    [SerializeField]private int _steps = 1;
    [SerializeField]private bool _canMove = false;
    public void Spawn()
    {
        _gridCell = GameObject.Find("Hexagons").GetComponent<GridCell>();
        _cell = _gridCell._hexagonGrid;
        Debug.Log($"Cell length: {_cell.GetLength(0)},{_cell.GetLength(1)}");
        _cellNumbers = new int[_cell.GetLength(0), _cell.GetLength(1)];
        Debug.Log($"Name of object: {_cell.Length}");
        Debug.Log($"X: {_goal.Item1},Y: {_goal.Item2}");
        _canMove = true;
        GetTagType();
        
    }
    private void GetObjectWay(int[,] grid, (int,int) start, (int, int) goal)
    {
        List<(int, int)> path = AStar.FindPath(grid, start, goal);
        _missleWay = path;
        
        if(_missleWay != null)
        {
            Debug.Log("Found a way");
            foreach(var step in _missleWay)
            {
              // Debug.Log($"{step.Item1},{step.Item2}");
            }
        }
        else
        {
            Debug.Log("Didn't find a way");
        }
        
    }
    private void ObjectMove()//Метод передвижения самолетов
    {
        
        if (_cell != null && _missleWay != null)
        {
            Debug.Log( _missleWay.Count+ " :Missle ways Count" );
            Vector3 tmpTransform = _cell[_missleWay[_steps].Item1, _missleWay[_steps].Item2].transform.position;
            Vector3 MoveDirection;
            if (_steps + 1 > _missleWay.Count - 1)
            {
                transform.LookAt(_cell[_missleWay[_steps].Item1, _missleWay[_steps].Item2].transform.position, Vector3.up);
            }
            else
            {
                transform.LookAt(_cell[_missleWay[_steps+1].Item1, _missleWay[_steps+1].Item2].transform.position, Vector3.up);
            }
            MoveDirection = new Vector3(tmpTransform.x - transform.position.x, 0, tmpTransform.z - transform.position.z).normalized;
            transform.position += MoveDirection * _speed * Time.deltaTime;
            Debug.Log(_steps);
        }
    }
    private void GetTagType()
    {
        for (int i = 0; i < _cell.GetLength(0); i++)
        {
            for(int j = 0; j < _cell.GetLength(1); j++)
            {
                if (_cell[i,j].tag == "Zone")
                {
                    _cellNumbers[i, j] = 0;
                }
                else if (_cell[i,j].tag == "DeadZone")
                {
                    _cellNumbers[i, j] = 1;
                }
            }
        }
    }
    private void Update()
    {
        if(_canMove) ObjectMove();
        //getObjectPosition();
    }
    public void getTargetObject(int X, int Y, Transform target)
    {
        _target = target;
        _goal.Item1 = X;
        _goal.Item2 = Y;
        GetObjectWay(_cellNumbers, _MissleCellPosition, _goal);
    }//метод который получает местоположение башни
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tower")
        {
            _target.GetComponent<Tower>().TakeDamage(_damage);
            Destroy(gameObject);
        }
        if (other.tag == "Zone" || other.tag == "WaterZone")
        {
            if (other.GetComponent<SixAngelSelection>() && other.GetComponent<SixAngelSelection>() != null && other.GetComponent<SixAngelSelection>().IndexX == _cell[_missleWay[_steps].Item1, _missleWay[_steps].Item2].GetComponent<SixAngelSelection>().IndexX &&
                other.GetComponent<SixAngelSelection>().IndexY == _cell[_missleWay[_steps].Item1, _missleWay[_steps].Item2].GetComponent<SixAngelSelection>().IndexY)
            {
                Debug.Log("Ok_1");
                transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                _canMove = false;
                _steps++;
                if (_steps < _missleWay.Count)
                {
                    Debug.Log("Ok_2");

                    StartCoroutine(WaitAndMove());
                }
                else
                {
                    Debug.Log("IDK_2");
                }
            }
            else
            {
                Debug.Log("IDK");
            }
            
        }
    }
    private IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(1.0f);
        _canMove = true;

    }

}
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