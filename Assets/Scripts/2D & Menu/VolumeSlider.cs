using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public PlayerStats playerStats;

    private void Update()
    {
        playerStats.Volume = slider.value;
    }
}
