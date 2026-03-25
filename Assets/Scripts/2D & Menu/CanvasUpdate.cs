
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private int maxArmor;
    [SerializeField] private Image armorBarImage;
    [SerializeField] private TextMeshProUGUI armorText;

    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI rockText;
    [SerializeField] private TextMeshProUGUI metalText;

    [SerializeField] private TextMeshProUGUI nightText;

    [SerializeField] private Canvas canvas;

    [SerializeField] private AudioSource bossFightMusic;
    [SerializeField] private AudioSource ForestAmbience;


    public PlayerStats playerStats;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "2D")
        {
            if (playerStats.BossFight)
            {
                canvas.enabled = false;
                bossFightMusic.Play();
            }
            else { ForestAmbience.Play(); }
        }
        maxHealth = 100;
        maxArmor = 100;
    }

    private void Update()
    {
        UpdateText();
        UpdateBars();
    }

    private void UpdateText()
    {

        if (healthText != null)
        {
            healthText.text = playerStats.Health + "/" + maxHealth;
        }
        if (armorText != null)
        {
            armorText.text = playerStats.Armor + "/" + maxArmor;
        }    
        if (woodText != null)
        {
            woodText.text = "" + playerStats.Wood;
        }
        if (rockText != null)
        {
            rockText.text = "" + playerStats.Rock;
        }
        if (metalText != null)
        {
            metalText.text = "" + playerStats.Metal;
        }

        if (SceneManager.GetActiveScene().name == "2D")
        {
            if (nightText != null)
            {
                nightText.text = "night:" + playerStats.Night;
            }
        }
        else
        {
            if (nightText != null)
            {
                nightText.text = "day:" + playerStats.Night;
            }
        }

    }


    private void UpdateBars()
    {
        float healthRatio = (float)playerStats.Health / (float)maxHealth;
        healthBarImage.fillAmount = healthRatio;

        float armorRatio = (float)playerStats.Armor / (float)maxArmor;
        armorBarImage.fillAmount = armorRatio;
    }
}
