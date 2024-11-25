using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private MissleCellController _missleCellContoroller;
    [SerializeField] private MissleController _missleLineContoroller;
    [Tooltip("Type of missle")]
    public string missleType;
    private void Start()
    {
        if(missleType == "cell") _missleCellContoroller = GetComponent<MissleCellController>();
        if(missleType == "line") _missleLineContoroller = GetComponent<MissleController>();

    }
    private void Update()
    {

        _target = GameObject.FindWithTag("Tower").GetComponent<Transform>();
        switch (missleType)
        {
            case "cell": Debug.Log($"{_target.GetComponent<Tower>().indexX}"); 
                _missleCellContoroller.getTargetObject(_target.GetComponent<Tower>().indexX, _target.GetComponent<Tower>().indexY, _target); break;

            case "line": _missleLineContoroller.getTargetObject(_target); break;
        }

    }
}
