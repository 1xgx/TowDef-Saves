using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHealth : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        _healthBar.value = currentValue/maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
