using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarDetection : MonoBehaviour
{
    [SerializeField] private Transform _objectTrigger;
    public List<AirDefenseController> _airDefenses;
    [SerializeField] private GameManager _gameManager;
    public List<GameObject> ECS;
    public bool ECSIsPosed;
    [SerializeField]
    AirDefenseController nearestAirDefense = new AirDefenseController();

    private void Start()
    {
        _gameManager = new GameManager();
    }
    private void Update()
    {
        
    }
    public void getDetectedObject(Transform Target)
    {
        if (ECS.Count > 0 && ECSIsPosed)
        {
            _objectTrigger = Target;
            FindNearestAirDefense();
            Debug.Log(nearestAirDefense.name);
            nearestAirDefense.GetComponent<AirDefenseController>().Shooting(_objectTrigger);
        }
        else
        {
            return;
        }
    }
    private void FindNearestAirDefense()
    {

        float minDistance = float.MaxValue;

        foreach (var airDefense in _airDefenses)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(airDefense.transform.position.x - transform.position.x, 2) 
                + Mathf.Pow(airDefense.transform.position.y - transform.position.y, 2));
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestAirDefense = airDefense;
            }
        }
    }
}
