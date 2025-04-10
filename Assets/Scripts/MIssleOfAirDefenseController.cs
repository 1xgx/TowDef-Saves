using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIssleOfAirDefenseController : MonoBehaviour
{
    public Transform Target;
    private float _damage = 10.0f;
    [SerializeField]private float _speed = 100f;

    private void FixedUpdate()
    {
        if(Target == null) 
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 MoveDirection = (Target.position - transform.position).normalized;
        transform.LookAt(Target, new Vector3(90, 1, 0));
        transform.position += MoveDirection * _speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Missle")
        {
            if (other.GetComponent<Missle>())
            {
                other.GetComponent<Missle>().TakeDamage(_damage);
            }
            //Destroy(other.gameObject);
            Destroy(gameObject);
            
        }
    }
}
