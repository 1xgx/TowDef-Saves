using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VehicleCellMovement : MonoBehaviour
{
    [Header("Stats")]
    public float Health, MaxHealth = 100.0f;
    public int indexX, indexY;
    [SerializeField] private Transform _targetHexagon;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage; 

    [Header("Cell Movement Settings")]
    private List<(int, int)> _vehicleWay;
    [SerializeField] private GridCell _gridCell;
    private int[,] _cellNumbers;
    private GameObject[,] _cell;
    private (int, int) _goal;
    public (int, int) VehiclePosition;
    [SerializeField] private int _steps = 1;
    [SerializeField] private bool _canMove = false;

    public void Spawn()
    {
        _gridCell = GameObject.Find("Hexagons").GetComponent<GridCell>();
        _cell = _gridCell._hexagonGrid;
        Debug.Log($"Cell length: {_cell.GetLength(0)},{_cell.GetLength(1)}");
        _cellNumbers = new int[_cell.GetLength(0), _cell.GetLength(1)];
        Debug.Log($"Name of object: {_cell.Length}");
        Debug.Log($"X: {_goal.Item1},Y: {_goal.Item2}");
        //_canMove = true;
        GetTagType();
    }
    public void GetChoosedHexagon(int X, int Y, Transform target)
    {
        _targetHexagon = target;
        _goal.Item1 = X;
        _goal.Item2 = Y;
        GetObjectWay(_cellNumbers, VehiclePosition, _goal);
    }
    private void GetObjectWay(int[,] grid, (int, int) start, (int, int) goal)
    {
        List<(int, int)> path = AStar.FindPath(grid, start, goal);
        _vehicleWay = path;

        if (_vehicleWay != null)
        {
            Debug.Log("Found a way");
            foreach (var step in _vehicleWay)
            {
                // Debug.Log($"{step.Item1},{step.Item2}");
            }
        }
        else
        {
            Debug.Log("Didn't find a way");
        }

    }
    private void GetTagType()
    {
        for (int i = 0; i < _cell.GetLength(0); i++)
        {
            for (int j = 0; j < _cell.GetLength(1); j++)
            {
                if (_cell[i, j].tag == "Zone")
                {
                    _cellNumbers[i, j] = 0;
                }
                else if (_cell[i, j].tag == "DeadZone")
                {
                    _cellNumbers[i, j] = 1;
                }
            }
        }
    }
}

