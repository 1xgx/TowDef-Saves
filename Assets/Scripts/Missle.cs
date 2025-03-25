using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private BuildingHealth healthBar;
    [SerializeField] private Transform _target;
    [SerializeField] private MissleCellController _missleCellContoroller;
    [SerializeField] private MissleController _missleLineContoroller;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private int _costOfDestroy = 25;
    [Tooltip("Type of missle")]
    public string missleType;
    private void Start()
    {
        _health = _maxHealth;
        if (missleType == "cell") _missleCellContoroller = GetComponent<MissleCellController>();
        if(missleType == "line") _missleLineContoroller = GetComponent<MissleController>();

    }
    private void LateUpdate()
    {
        if (_gameManager == null) _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (_target == null && _gameManager != null) _target = _gameManager.randomElectroStationForMissle();
        switch (missleType)
        {
            case "cell":
                if (_target == null) _target = _gameManager.randomElectroStationForMissle();
                if (_target.GetComponent<ElectroStation>()) 
                {
                    Debug.Log($"{_target.GetComponent<ElectroStation>().indexX}");
                    _missleCellContoroller.getTargetObject(_target.GetComponent<ElectroStation>().indexX, _target.GetComponent<ElectroStation>().indexY, _target);
                }
                if (_target.GetComponent<SubElectroStation>())
                {
                    Debug.Log($"{_target.GetComponent<SubElectroStation>().indexX}");
                    _missleCellContoroller.getTargetObject(_target.GetComponent<SubElectroStation>().indexX, _target.GetComponent<SubElectroStation>().indexY, _target);
                }
                break;

            case "line": if (_target == null) _target = _gameManager.randomElectroStationForMissle();
                _missleLineContoroller.getTargetObject(_target); break;
        }

    }
    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        if(healthBar != null)healthBar.UpdateHealthBar(_health, _maxHealth);
        if (_health <= 0)
        {
            _gameManager.sendMoney(_costOfDestroy);
            Destroy(gameObject);
        }
    }
}
