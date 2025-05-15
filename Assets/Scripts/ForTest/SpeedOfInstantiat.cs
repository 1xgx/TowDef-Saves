using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedOfInstantiat : MonoBehaviour
{
    [SerializeField] private MissleSpawner _missleSpawner;
    [SerializeField] private Slider _slider;

    public void Change()
    {
        _missleSpawner._delay = _slider.value;
    }
}
