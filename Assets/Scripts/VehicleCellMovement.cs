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
    public int IndexX, IndexY;
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
    [Header ("Object Rotates")]
    [SerializeField] private Transform _objectRotate;
    [Header("Missle Setting")]
    [SerializeField] private float _reloadDelay = 5.0f;
    [SerializeField] private float _delayBetweenFire = 1.0f;
    [SerializeField] private bool _isReloading = false;
    [SerializeField] private int _bullets = 5;
    [SerializeField] private int _bulletsConst;
    [SerializeField] private GameObject _refBullet;
    [SerializeField] private bool _flagFirst = true;
    [SerializeField] private bool _flagSecond = false;
    [SerializeField] private Transform _target;
    public void Spawn()
    {
        _gridCell = GameObject.Find("Hexagons").GetComponent<GridCell>();
        _cell = _gridCell._hexagonGrid;
        Debug.Log($"Cell length: {_cell.GetLength(0)},{_cell.GetLength(1)}");
        _cellNumbers = new int[_cell.GetLength(0), _cell.GetLength(1)];
        Debug.Log($"Name of object: {_cell.Length}");
        //_canMove = true;
        VehiclePosition.Item1 = IndexX;
        VehiclePosition.Item2 = IndexY;
        GetTagType();
    }
    private void Awake()
    {
        _bulletsConst = _bullets;
    }
    private void Update()
    {
        if (_canMove) 
        {
            gameObject.GetComponent<BoxCollider>().size = new Vector3(0.7f,gameObject.GetComponent<BoxCollider>().size.y + .7f,0.6f);
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            ObjectMove(); 
        }
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
            Debug.Log("Found a way" + gameObject.name + "! ! !");
            foreach (var step in _vehicleWay)
            {
                Debug.Log($"{step.Item1},{step.Item2}");
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
    private void ObjectMove()//Метод передвижения 
    {

        if (_cell != null && _vehicleWay != null)
        {
            Debug.Log(_vehicleWay.Count + " :Missle ways Count");
            Vector3 tmpTransform = _cell[_vehicleWay[_steps].Item1, _vehicleWay[_steps].Item2].transform.position;
            Vector3 MoveDirection;
            if (_steps + 1 > _vehicleWay.Count - 1)
            {
                transform.LookAt(_cell[_vehicleWay[_steps].Item1, _vehicleWay[_steps].Item2].transform.position, Vector3.up);
                transform.Rotate(0, 180, 0);
            }
            else
            {
                transform.LookAt(_cell[_vehicleWay[_steps + 1].Item1, _vehicleWay[_steps + 1].Item2].transform.position, Vector3.up);
                transform.Rotate(0, 180, 0);
            }
            MoveDirection = new Vector3(tmpTransform.x - transform.position.x, 0, tmpTransform.z - transform.position.z).normalized;
            transform.position += MoveDirection * _speed * Time.deltaTime;
            Debug.Log(_steps);
        }
    }
    private void Shooting()
    {
        
        if (_target != null)
        {

            //_target = GameObject.FindWithTag("Missle").transform;
            _objectRotate.LookAt(_target.transform, Vector3.down);
            _objectRotate.Rotate(0, 180, 0);
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
        newBullet.GetComponent<MIssleOfAirDefenseController>().Target = _target;
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
    private IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(1.0f);
        _canMove = true;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missle") 
        {
            _target = other.GetComponent<Transform>();

            Debug.Log($"target {_target}");
        }
        if (other.tag == "Zone" || other.tag == "WaterZone")
        {
            if (other.GetComponent<SixAngelSelection>() && other.GetComponent<SixAngelSelection>() != null && other.GetComponent<SixAngelSelection>().IndexX == _cell[_vehicleWay[_steps].Item1, _vehicleWay[_steps].Item2].GetComponent<SixAngelSelection>().IndexX &&
                other.GetComponent<SixAngelSelection>().IndexY == _cell[_vehicleWay[_steps].Item1, _vehicleWay[_steps].Item2].GetComponent<SixAngelSelection>().IndexY)
            {
                Debug.Log("Ok_1");
                transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                _canMove = false;
                _steps++;
                if (_steps < _vehicleWay.Count)
                {
                    Debug.Log("Ok_2");

                    StartCoroutine(WaitAndMove());
                }
                else
                {
                    GameObject.FindWithTag("Player").GetComponent<PlayerController>()._selectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject = gameObject;
                    GameObject.FindWithTag("Player").GetComponent<PlayerController>()._selectedObject = null;
                }
            }
            else
            {
                Debug.Log("IDK");
            }

        }
    }
}

