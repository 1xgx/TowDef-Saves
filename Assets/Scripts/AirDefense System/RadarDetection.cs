using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarDetection : MonoBehaviour
{
    [SerializeField] private Transform _objectTrigger;
    public List<AirDefenseController> _airDefenses;
    [SerializeField] private GameManager _gameManager;
    public List<GameObject> ECS;
    [SerializeField]
    AirDefenseController nearestAirDefense = new AirDefenseController();

    private void Start()
    {
        _gameManager = new GameManager();
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missle")
        {
            if (ECS.Count > 0)
            {
                _objectTrigger = other.GetComponent<Transform>();
                FindNearestAirDefense();
                Debug.Log(nearestAirDefense.name);
                nearestAirDefense.GetComponent<AirDefenseController>().Shooting(_objectTrigger);
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _objectTrigger = null;
        Debug.Log("Undetected");
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
