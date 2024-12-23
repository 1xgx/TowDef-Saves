using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AirDefenseController : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _selectedTarget;
    [SerializeField] private GameObject _missle;
    [SerializeField] private GameObject _missleSpawned;
    [SerializeField] private List<GameObject> _misslesSpawned;
    [SerializeField] private float _missleSpeed;

    [Header("Bullets")]
    [SerializeField]
    private int _bullets;
    private int _bulletsConst;
    private bool _isReloading = false;
    private float _reloadDelay;

    private void Awake()
    {
        _bulletsConst = _bullets;
    }

    private void Update()
    {
        Shooting();
    }
    private void Shooting()
    {
        bool isFound = GameObject.FindWithTag("Missle");
        if (isFound)
        {
            _target = GameObject.FindWithTag("Missle");
            transform.LookAt(_target.transform, Vector3.up);
        }
    }

    private void MissleSpawner()
    {
        //Spawn missle
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
