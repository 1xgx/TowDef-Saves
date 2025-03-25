using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleController : MonoBehaviour
{
    private Transform _target;
    [Header ("Standart option")]
    [SerializeField] private float _health, _maxHealth = 3.0f;
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _damage = 3.0f;
    [SerializeField] private BuildingHealth healthBar;
    private GameObject _player;
    



    private void Awake()
    {
        healthBar = GetComponentInChildren<BuildingHealth>();

    }
    private void Start()
    {
        _health = _maxHealth;
    }
    public void getTargetObject(Transform target)
    {
        _target = target;
        transform.LookAt(_target, Vector3.up);
        Vector3 MoveDirection = (_target.position - transform.position).normalized;
        transform.position += MoveDirection * _speed * Time.deltaTime;
    }
    
    
    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_target.tag != other.tag) return;
        if (other.tag == "Tower")
        {
            _target.GetComponent<ElectroStation>().TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if (other.tag == "SubTower")
        {
            _target.GetComponent<SubElectroStation>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
