using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElectroStation : MonoBehaviour
{
    public static float _health, _maxHealth = 1000.0f;
    public int indexX, indexY;
    [SerializeField] private BuildingHealth healthBar;
    
    private void Awake()
    {
        healthBar = GetComponentInChildren<BuildingHealth>();
    }
    private void Start()
    {
        _health = _maxHealth;
    }
    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        healthBar.UpdateHealthBar(_health, _maxHealth);
        if (_health <= 0)
        {
            GameManager tmpGameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            for(int i = 0; i < tmpGameManager.Towers.Count; i++)
            {
                if(gameObject == tmpGameManager.Towers[i])
                {
                    tmpGameManager.Towers.RemoveAt(i);
                    Die();
                }
            }
        }
    }
    private void Die() 
    {
        SceneManager.LoadSceneAsync(0);

    }
}
