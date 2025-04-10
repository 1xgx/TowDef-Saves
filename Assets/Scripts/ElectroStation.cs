using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElectroStation : MonoBehaviour
{
    public static float _health;
    public static float _maxHealth = 100.0f;
    public int indexX, indexY;
    [SerializeField] private BuildingHealth healthBar;
    [SerializeField] private ObjectType _type;
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
                if(tmpGameManager.Towers[i].GetComponent<ElectroStation>())
                {
                    tmpGameManager.Towers.RemoveAt(i);
                    tmpGameManager.DieSound(_type);
                    Die();
                }
            }
        }
    }
    private void Die() 
    {
        Debug.Log("Hello World");
        SceneManager.LoadSceneAsync(3);

    }
}
