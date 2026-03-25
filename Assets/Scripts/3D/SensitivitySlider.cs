using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider slider;
    public PlayerStats playerStats;

    private void Start()
    {
        slider.value = slider.maxValue / 2;   
    }

    void Update()
    {
        playerStats.Sensitivity = slider.value;
    }
}
