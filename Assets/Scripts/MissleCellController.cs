using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MissleCellController : MonoBehaviour
{
    //[SerializeField] private Transform[] _allHexagons;
    //[SerializeField] private Transform[,] _allHexagons2DArray;
    //private List<Transform> _path;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Transform _target;

    [SerializeField] private float _damage = 3.0f;

    [SerializeField] private GridCell _gridCell;
    private int[,] _cellNumbers;
    private GameObject[,] _cell;
    private List<(int, int)> _missleWay;
    public (int, int) MissleCellPosition;
    private (int, int) _goal;
    [SerializeField]private int _steps = 1;
    [SerializeField]private bool _canMove = false;
    //Health
    [Header("Health")]
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private BuildingHealth healthBar;
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
        //healthBar = GetComponentInChildren<BuildingHealth>();
        _health = _maxHealth;

    }
    private void Awake()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
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
        GetObjectWay(_cellNumbers, MissleCellPosition, _goal);
    }//метод который получает местоположение башни
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tower")
        {
            _target.GetComponent<ElectroStation>().TakeDamage(_damage);
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
    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        healthBar.UpdateHealthBar(_health, _maxHealth);
        if (_health <= 0)
        {
            Destroy(gameObject);

        }
    }
    private IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(1.0f);
        _canMove = true;

    }

}