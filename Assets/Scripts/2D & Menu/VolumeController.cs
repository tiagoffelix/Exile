using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public PlayerStats playerStats;

    private void Update()
    {
        if (playerStats != null)
        {
            AudioListener.volume = playerStats.Volume;
        }
    }
}
