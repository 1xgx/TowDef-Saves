using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubElectroStation : MonoBehaviour
{
    public static float _health, _maxHealth = 25.0f;
    public int indexX, indexY;
    [SerializeField] private BuildingHealth healthBar;
    public GameObject ElectroStation;
    private void Awake()
    {
        healthBar = GetComponentInChildren<BuildingHealth>();
    }
    void Start()
    {
        _health = _maxHealth;
    }

    // Update is called once per frame
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
        ElectroStation.GetComponent<ElectroStation>().TakeDamage(25f);
        Debug.Log("SubElectrostation is Died");
        Destroy(gameObject);
    }
}
