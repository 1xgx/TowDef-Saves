using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIssleOfAirDefenseController : MonoBehaviour
{
    public Transform Target;
    private float _damage = 10.0f;
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
            if (other.GetComponent<MissleCellController>())
            {
                other.GetComponent<MissleCellController>().TakeDamage(10.0f);
            }
            //Destroy(other.gameObject);
            Destroy(gameObject);
            
        }
    }
}
