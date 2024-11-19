using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AirDefenseController : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _selectedTarget;
    // Update is called once per frame
    void Update()
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
}
