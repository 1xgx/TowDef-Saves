using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIssleOfAirDefenseController : MonoBehaviour
{
    public Transform Target;
    [SerializeField]private float _speed = 100f;

    private void Update()
    {
        if(Target == null) 
        {
            Destroy(gameObject);
            return;
        }
        transform.LookAt(Target, Vector3.up);
        Vector3 MoveDirection = (Target.position - transform.position).normalized;
        transform.position += MoveDirection * _speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Missle")
        {
           
            Destroy(other.gameObject);
            Destroy(gameObject);
            
        }
    }
}
