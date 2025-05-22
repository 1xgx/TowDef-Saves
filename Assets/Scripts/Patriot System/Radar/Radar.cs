using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Radar : MonoBehaviour
{
    private GameObject GetTrigger;
    private List<GameObject> Triggers;
    public int indexX, indexY;
    public static float _health, _maxHealth = 125.0f;
    [SerializeField] private BuildingHealth healthBar;
    public List<GameObject> ECS;
    // Start is called before the first frame update
    private void Awake()
    {
        healthBar = GetComponentInChildren<BuildingHealth>();
        List<GameObject> Triggers = new List<GameObject>();
        _health = _maxHealth;
    }
    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        healthBar.UpdateHealthBar(_health, _maxHealth);
        if (_health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        SceneManager.LoadSceneAsync(0);
        Destroy(gameObject);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "missle")
        {
            GetTrigger = other.GetComponent<GameObject>();
            Triggers.Add(GetTrigger);
        }
        GetTrigger = null;
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
