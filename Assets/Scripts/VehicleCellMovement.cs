using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
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
    public bool SelectedVehicle = false;
    [SerializeField] private int _steps = 1;
    [SerializeField] private bool _canMove = false;
    [Header("Missle Setting")]
    [SerializeField] private float _reloadDelay = 5.0f;
    [SerializeField] private float _delayBetweenFire = 1.0f;
    [SerializeField] private bool _isReloading = false;
    [SerializeField] private int _bullets = 5;
    [SerializeField] private int _bulletsConst;
    [SerializeField] private GameObject _refBullet;
    [SerializeField] private bool _flagFirst = true;
    [SerializeField] private bool _flagSecond = false;
    [SerializeField] private GameObject _target;
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
    private void Awake()
    {
        _bulletsConst = _bullets;
    }
    private void Update()
    {
        Shooting();
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
    private void Shooting()
    {
        bool isFound = GameObject.FindWithTag("Missle");
        if (isFound)
        {

            _target = GameObject.FindWithTag("Missle");
            transform.LookAt(_target.transform, Vector3.up);
            if (_flagFirst && !_flagSecond) MissleSpawner();
        }
    }
    private void MissleSpawner()
    {
        if (_bullets < 0)
        {
            StopCoroutine(QueueOfBullets());
            StartCoroutine(ReloadBullets());

        }
        
        _flagSecond = true;
        
        if(_bullets > 0)StartCoroutine(QueueOfBullets());
        //Spawn missle
    }

    private IEnumerator QueueOfBullets()
    {
        GameObject newBullet = Instantiate(_refBullet, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        if (_target.IsDestroyed())
        {
            _flagSecond = false;
            yield break;
        }
        if(_bullets < 0)
        {
            _flagSecond = false;
            yield break;
        }
        Debug.Log(_target.name);
        newBullet.GetComponent<MIssleOfAirDefenseController>().Target = _target.transform;
        _bullets--;
        yield return new WaitForSeconds(_delayBetweenFire);
        MissleSpawner();
    }
    private IEnumerator ReloadBullets()
    {
        _isReloading = true;
        Debug.Log("Reload start...");

        yield return new WaitForSeconds(_reloadDelay);

        _bullets = _bulletsConst;
        Debug.Log("Reload end...");

        _isReloading = false;
    }
}

