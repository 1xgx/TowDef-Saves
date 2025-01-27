using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AirDefenseController : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _selectedTarget;
    [SerializeField] private GameObject _missle;
    [SerializeField] private GameObject _missleSpawned;
    [SerializeField] private List<GameObject> _misslesSpawned;
    [Header("Bullets")]
    [SerializeField]
    private int _bullets;
    [SerializeField]
    private GameObject _refBullet;
    private int _bulletsConst;
    private bool _isReloading = false;
    private float _reloadDelay;
    [Header("Test place")]
    [SerializeField] private bool flag = true;
    [SerializeField] private bool flag2 = false;
    private void Awake()
    {
        _bulletsConst = _bullets;
    }

    private void Update()
    {

    }
    public void Shooting(Transform target)
    {
        Transform TestTarget = GameObject.FindWithTag("Missle").transform;
        bool isFound = false;
        if (isFound)
        { 
            _target = TestTarget;
            transform.LookAt(_target.transform, Vector3.up);
            if (flag && !flag2) MissleSpawner();
        }
        _target = target;
        if(target.tag == "Missle")
        {
            transform.LookAt(target.transform, Vector3.up);
            if (flag && !flag2) MissleSpawner();
        }
    }

    private void MissleSpawner()
    {
        flag2 = true;
        if (_bullets == 0)
        {
            StopCoroutine(QueueOfBullets());
            StartCoroutine(ReloadBullets());

        }
        StartCoroutine(QueueOfBullets());
        //Spawn missle
    }
    
    private IEnumerator QueueOfBullets()
    {
        GameObject newBullet = Instantiate(_refBullet, new Vector3(transform.position.x,0, transform.position.z), Quaternion.identity);
        if (_target.IsDestroyed())
        {
            flag2 = false;
            yield break;
        }
        Debug.Log(_target.name);
        newBullet.GetComponent<MIssleOfAirDefenseController>().Target = _target.transform;
        _bullets--;
        yield return new WaitForSeconds(1.0f);
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
