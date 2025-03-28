using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLight : MonoBehaviour
{
    [SerializeField] private GameObject _pointLight;
    public void Night(bool night)
    {
        if (night)
        {
            _pointLight.SetActive(true);
        }
        else
        {
            _pointLight.SetActive(false);
        }
    }
}
